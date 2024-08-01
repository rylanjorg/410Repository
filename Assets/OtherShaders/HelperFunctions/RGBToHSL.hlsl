void RGBToHSL_float(float4 c, out float Hue, out float Saturation, out float Lightness, out float Alpha)
{
    float4 hsl = 0;
    float maxVal = max(max(c.r, c.g), c.b);
    float minVal = min(min(c.r, c.g), c.b);
    float delta = maxVal - minVal;
    
    hsl.z = (maxVal + minVal) / 2.0; // Luminance
    
    if (delta != 0)
    {
        if (hsl.z < 0.5)
        {
            hsl.y = delta / (maxVal + minVal); // Saturation
        }
        else
        {
            hsl.y = delta / (2.0 - maxVal - minVal); // Saturation
        }
        
        float deltaR = (((maxVal - c.r) / 6.0) + (delta / 2.0)) / delta;
        float deltaG = (((maxVal - c.g) / 6.0) + (delta / 2.0)) / delta;
        float deltaB = (((maxVal - c.b) / 6.0) + (delta / 2.0)) / delta;
        
        if (c.r == maxVal)
        {
            hsl.x = deltaB - deltaG; // Hue
        }
        else if (c.g == maxVal)
        {
            hsl.x = (1.0 / 3.0) + deltaR - deltaB; // Hue
        }
        else if (c.b == maxVal)
        {
            hsl.x = (2.0 / 3.0) + deltaG - deltaR; // Hue
        }
        
        if (hsl.x < 0.0)
        {
            hsl.x += 1.0;
        }
        else if (hsl.x > 1.0)
        {
            hsl.x -= 1.0;
        }
    }

    Hue = hsl.x;
    Saturation = hsl.y;
    Lightness = hsl.z;
    Alpha = hsl.a;

}
