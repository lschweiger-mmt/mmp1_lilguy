// pass value from script via uniforms
uniform vec2 circlePosition;
uniform vec2 windowSize;
uniform float radius;
uniform vec4 color;

void main()
{
    vec4 light;
    vec2 screenRatio = vec2(1, windowSize.x / windowSize.y);
    float newRadius = radius / 64.0;

    float distFromLight = distance((gl_TexCoord[0].xy / screenRatio) / newRadius, ((((circlePosition + windowSize/2.0)) / windowSize)/screenRatio) / newRadius);
    if(distFromLight < 32.0)
        light = vec4( 0, 0, 0, 0);
    else
        light = color;

    
    gl_FragColor = gl_Color * light;
}
