//#define DEBUG_IMPORT

// Copyright (C) 2015 Jaroslav Stehlik - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SVGImporter.Geometry
{
    using Rendering;
    using Data;
    using Utils;

    public class SVGMesh
    {               
        public static Mesh CombineMeshes(List<SVGLayer> layers, out Shader[] shaders, SVGUseGradients useGradients = SVGUseGradients.Always, SVGAssetFormat format = SVGAssetFormat.Transparent, bool compressDepth = true)
        {
            #if DEBUG_IMPORT
            long timeStart = System.DateTime.Now.Ticks;
            #endif

            shaders = new Shader[0];

            //if(SVGAssetImport.sliceMesh) Create9Slice();

            SVGFill fill;
            bool useOpaqueShader = false;
            bool useTransparentShader = false;
            bool hasGradients = (useGradients == SVGUseGradients.Always);

            int totalLayers = layers.Count, totalTriangles = 0, opaqueTriangles = 0, transparentTriangles = 0;
            FILL_BLEND lastBlendType = FILL_BLEND.ALPHA_BLENDED;

            // Z Sort meshes
            if(format == SVGAssetFormat.Opaque)
            {
                SVGLayer layer;
                if(compressDepth)
                {
                    SVGBounds quadTreeBounds = SVGBounds.InfiniteInverse;
                    for (int i = 0; i < totalLayers; i++)
                    {
                        layer = layers[i];
                        if (layer.bounds.size.sqrMagnitude == 0f) continue;
                        quadTreeBounds.Encapsulate(layer.bounds.center, layer.bounds.size);
                    }

                    quadTreeBounds.size *= 1.2f;

                    if(!quadTreeBounds.isInfiniteInverse)
                    {
                        SVGDepthTree depthTree = new SVGDepthTree(quadTreeBounds);

                        for (int i = 0; i < totalLayers; i++)
                        {                           
                            layer = layers[i];
                            int[] nodes = depthTree.TestDepthAdd(i, new SVGBounds(layer.bounds.center, layer.bounds.size));

                            int nodesLength = 0;
                            if(nodes == null || nodes.Length == 0)
                            {
                                layer.depth = 0;
                            } else {
                                nodesLength = nodes.Length;
                                int highestDepth = 0;
                                int highestLayer = -1;

                                for(int j = 0; j < nodesLength; j++)
                                {
                                    if(layers[nodes[j]].depth > highestDepth)
                                    {
                                        highestDepth = layers[nodes[j]].depth;
                                        highestLayer = nodes[j];
                                    }
                                }

                                if(layers[i].fill.blend == FILL_BLEND.OPAQUE)
                                {
                                    layer.depth = highestDepth + 1;
                                } else {
                                    if(highestDepth != -1 && layers[highestLayer].fill.blend == FILL_BLEND.OPAQUE)
                                    {
                                        layer.depth = highestDepth + 1;
                                    } else {
                                        layer.depth = highestDepth;
                                    }
                                }
                            }

                            layers[i] = layer;
                        }
                    }
                } else {
                    int highestDepth = 0;
                    for (int i = 0; i < totalLayers; i++)
                    {
                        layer = layers[i];
                        fill = layer.fill;
                        if (fill.blend == FILL_BLEND.OPAQUE || lastBlendType == FILL_BLEND.OPAQUE)
                        {
                            layer.depth = ++highestDepth;
                        } else 
                        {
                            layer.depth = highestDepth;
                        }
                        
                        lastBlendType = fill.blend;
                        layers[i] = layer;
                    }
                }
            }

            int totalVertices = 0, vertexCount, vertexStart = 0, currentVertex;
            for(int i = 0; i < totalLayers; i++)
            {
                fill = layers[i].fill;
                if(fill.blend == FILL_BLEND.OPAQUE) { 
                    opaqueTriangles += layers[i].triangles.Length;
                    useOpaqueShader = true; 
                }
                else if(fill.blend == FILL_BLEND.ALPHA_BLENDED) { 
                    transparentTriangles += layers[i].triangles.Length;
                    useTransparentShader = true; 
                }
                if(fill.fillType == FILL_TYPE.GRADIENT) hasGradients = true;
                vertexCount = layers[i].vertices.Length;
                totalVertices += vertexCount;
            }

            totalTriangles = opaqueTriangles + transparentTriangles;

            if(useGradients == SVGUseGradients.Never) hasGradients = false;
            if(format != SVGAssetFormat.Opaque)
            { 
                useOpaqueShader = false; 
                useTransparentShader = true;
            }

            Vector3[] vertices = new Vector3[totalVertices];
            Color32[] colors32 = new Color32[totalVertices];
            Vector2[] uv = null;
            Vector2[] uv2 = null;
            int[][] triangles = null;
            List<Shader> outputShaders = new List<Shader>();

            if(hasGradients)
            {
                uv = new Vector2[totalVertices];
                uv2 = new Vector2[totalVertices];

                if(useOpaqueShader)
                {
                    outputShaders.Add(SVGShader.GradientColorOpaque);
                }
                if(useTransparentShader)
                {
                    outputShaders.Add(SVGShader.GradientColorAlphaBlended);
                }
            } else {
                if(useOpaqueShader)
                {
                    outputShaders.Add(SVGShader.SolidColorOpaque);
                }
                if(useTransparentShader)
                {
                    outputShaders.Add(SVGShader.SolidColorAlphaBlended);
                }
            }

            for(int i = 0; i < totalLayers; i++)
            {
                vertexCount = layers[i].vertices.Length;
//                Debug.Log("Layer: "+i+", depth: "+layers[i].depth);
                for(int j = 0; j < vertexCount; j++)
                {
                    currentVertex = vertexStart + j;
                    vertices[currentVertex] = layers[i].vertices[j];
                    vertices[currentVertex].z = layers[i].depth * -SVGAssetImport.minDepthOffset;
                    colors32[currentVertex] = layers[i].fill.finalColor;
                }

                if(hasGradients && layers[i].fill.fillType == FILL_TYPE.GRADIENT && layers[i].fill.gradientColors != null)
                {
                    SVGMatrix svgFillTransform = layers[i].fill.transform;
                    Rect viewport = layers[i].fill.viewport;

                    Vector2 uvPoint = Vector2.zero;
                    Vector2 uv2Value = new Vector2(layers[i].fill.gradientColors.index, (int)layers[i].fill.gradientType);

                    for(int j = 0; j < vertexCount; j++)
                    {
                        currentVertex = vertexStart + j;
                        uvPoint.x = vertices [currentVertex].x;
                        uvPoint.y = vertices [currentVertex].y;
                        uvPoint = svgFillTransform.Transform(uvPoint);
                        uv[currentVertex].x = (uvPoint.x - viewport.x) / viewport.width;
                        uv[currentVertex].y = (uvPoint.y - viewport.y) / viewport.height;
                        uv2[currentVertex] = uv2Value;
                    }
                }

                vertexStart += vertexCount;
            }
            /*
            for(int i = 0; i < totalVertices; i++)
            {
                vertices[i] *= SVGAssetImport.meshScale;
            }
            */
            if(useOpaqueShader && useTransparentShader)
            {
                triangles = new int[2][]{new int[opaqueTriangles], new int[transparentTriangles]};

                int lastVertexIndex = 0;
                int triangleCount;
                int lastOpauqeTriangleIndex = 0;
                int lastTransparentTriangleIndex = 0;
                
                for(int i = 0; i < totalLayers; i++)
                {
                    triangleCount = layers[i].triangles.Length;
                    if(layers[i].fill.blend == FILL_BLEND.OPAQUE)
                    {
                        for(int j = 0; j < triangleCount; j++)
                        {
                            triangles[0][lastOpauqeTriangleIndex++] = lastVertexIndex + layers[i].triangles[j];
                        }
                    } else {
                        for(int j = 0; j < triangleCount; j++)
                        {
                            triangles[1][lastTransparentTriangleIndex++] = lastVertexIndex + layers[i].triangles[j];
                        }
                    }

                    lastVertexIndex += layers[i].vertices.Length;
                }
            } else {
                triangles = new int[1][]{new int[totalTriangles]};
                
                int lastVertexIndex = 0;
                int triangleCount;
                int lastTriangleIndex = 0;
                
                for(int i = 0; i < totalLayers; i++)
                {
                    triangleCount = layers[i].triangles.Length;
                    for(int j = 0; j < triangleCount; j++)
                    {
                        triangles[0][lastTriangleIndex++] = lastVertexIndex + layers[i].triangles[j];
                    }
                    lastVertexIndex += layers[i].vertices.Length;
                }
            }

            if(outputShaders.Count != 0) shaders = outputShaders.ToArray();

            /* * * * * * * * * * * * * * * * * * * * * * * * 
             *                                             *
             *      Mesh Creation                          *
             *                                             *
             * * * * * * * * * * * * * * * * * * * * * * * */

            Mesh output = new Mesh();
            output.vertices = vertices;
            output.colors32 = colors32;

            if(hasGradients)
            {
                output.uv = uv;
                output.uv2 = uv2;
            }

            if(triangles.Length == 1)
            {
                output.triangles = triangles[0];
            } else {
                output.subMeshCount = triangles.Length;
                for(int i = 0; i < triangles.Length; i++)
                {
                    output.SetTriangles(triangles[i], i);
                }
            }

            #if DEBUG_IMPORT
            System.TimeSpan timeSpan = new System.TimeSpan(System.DateTime.Now.Ticks - timeStart);
            Debug.Log("Mesh combination took: "+timeSpan.TotalSeconds +"s");
            #endif

            return output;
        }
        /*
        protected static void Create9Slice()
        {
            int meshCount = SVGGraphics.layers.Count;
            SVGBounds meshBounds = SVGBounds.InfiniteInverse;
            for (int i = 0; i < meshCount; i++)
            {
                if (SVGGraphics.layers [i].size.sqrMagnitude == 0f) continue;                
                meshBounds.Encapsulate(SVGGraphics.layers [i].center, SVGGraphics.layers [i].size);
            }

            // 9-slice
            if(SVGAssetImport.border.sqrMagnitude > 0f)
            {
                Vector2 min = meshBounds.min;
                Vector2 max = meshBounds.max;

                float bottom = Mathf.Lerp(min.y, max.y, 0.5f);

                for(int i = 0; i < meshCount; i++)
                {
                    if(SVGAssetImport.border.x > 0)
                        SVGMeshCutter.MeshSplit(SVGGraphics.layers [i], new Vector2(Mathf.Lerp(min.x, max.x, SVGAssetImport.border.x), 0f), Vector2.up); 
                    if(SVGAssetImport.border.y > 0)
                        SVGMeshCutter.MeshSplit(SVGGraphics.layers [i], new Vector2(0f, bottom), Vector2.right);                     
                    if(SVGAssetImport.border.z > 0)
                        SVGMeshCutter.MeshSplit(SVGGraphics.layers [i], new Vector2(Mathf.Lerp(min.x, max.x, 1f - SVGAssetImport.border.z), 0f), Vector2.up);                     
                    if(SVGAssetImport.border.w > 0)
                        SVGMeshCutter.MeshSplit(SVGGraphics.layers [i], new Vector2(0f, Mathf.Lerp(min.y, max.y, SVGAssetImport.border.w)), Vector2.right); 
                }
            }
        }
        */
    }
}
