uniform float offset;
uniform vec2 windowSize;
uniform vec4 colorA;
uniform vec4 colorB;
uniform vec2 mousePos;
float Circle(vec2 pos, float radius)
{
    return length(pos) - radius;
}

vec2 rotate(vec2 invec, float angle){
    return vec2(
        cos(angle) * invec.x - sin(angle) * invec.y,
        sin(angle) * invec.x + cos(angle) * invec.y
    );

}
void main()
{
    
    vec2 mouseCoord = mousePos* vec2(1,-1) + vec2(0, windowSize.y);

    float aspectRatio = 1.0 / min(windowSize.x, windowSize.y);
    vec2 uv = gl_FragCoord.xy * aspectRatio;


    float frequency = 18.0;
    float intensity = 1.5;
    float rotation = 7.0;
    float bulge = 3.0;
    float bulgeSize = 20.0;
    float radius = 0.03;

    uv = rotate(uv, rotation*0.0174533); // deg to rad
    vec2 frequencyChange = ((smoothstep(-1.0/bulge,1.0/bulge,(gl_FragCoord.xy-mouseCoord.xy)/(windowSize.xy)))-.5)*2.0*intensity;
    frequencyChange /= (length(gl_FragCoord.xy-mouseCoord.xy)/(windowSize.xy)*bulgeSize)+1.0;

    vec2 pos = fract(uv * (frequency - frequencyChange)) ;
    pos -= 0.5;

    float c = Circle(pos, radius);

    float s = 0.10 * aspectRatio;
    c = smoothstep(s,-s,c);

    gl_FragColor = mix(colorA, colorB, c);
}


