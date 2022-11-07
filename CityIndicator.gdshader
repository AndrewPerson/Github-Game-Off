shader_type canvas_item;

uniform sampler2D colours;
uniform float selectedStart;
uniform float selectedEnd;

vec2 polar_coordinates(vec2 uv)
{
	float radius = length(uv);
	float angle = (atan(uv.y, uv.x) / PI + 1.0) / 2.0;
	return vec2(radius, angle);
}

void fragment() {
	vec2 shiftedUV = (UV - vec2(0.5)) * 2.0;
	
	vec2 polarUV = polar_coordinates(shiftedUV);
	
	float radius = polarUV.y > selectedStart && polarUV.y < selectedEnd ? 1.0 : 0.9;
	
	if (polarUV.x <= radius) {
		COLOR = texture(colours, vec2(polarUV.y, 0));
	}
	else {
		COLOR = vec4(0);
	}
}
