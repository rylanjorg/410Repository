
#ifndef UNITY_INCLUDE_USETESS
#define UNITY_INCLUDE_USETESS

    // pre tesselation vertex program
    ControlPoint TessellationVertexProgram(Attributes v)
    {
        ControlPoint p;

        p.vertex = v.positionOS;
        p.uv = v.staticLightmapUV;
        p.normal = v.normalOS;
        //p.color = v.color;

        return p;
    }


    [UNITY_domain("tri")]
    Varyings domain(TessellationFactors factors, OutputPatch<ControlPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
    {
        Attributes v;

    #define DomainPos(fieldName) v.fieldName = \
                patch[0].fieldName * barycentricCoordinates.x + \
                patch[1].fieldName * barycentricCoordinates.y + \
                patch[2].fieldName * barycentricCoordinates.z;

            DomainPos(vertex)
            DomainPos(uv)
            //DomainPos(color)
            DomainPos(normal)

            return vert(v);
    }

#endif
