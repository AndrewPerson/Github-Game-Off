shader_type canvas_item;

uniform vec4 colour : source_color;
uniform float percentage;
uniform float percentage_offset;

vec2 polar_coordinates(vec2 uv)
{
	float radius = length(uv);
	float angle = (atan(uv.y, uv.x) / PI + 1.0) / 2.0;
	return vec2(radius, angle);
}

void fragment() {
	vec2 shiftedUV = (UV - vec2(0.5)) * 2.0;
	
	vec2 polarUV = polar_coordinates(shiftedUV);
	
	if (polarUV.x <= 1.0 && polarUV.y > percentage_offset && polarUV.y < percentage + percentage_offset) {
		COLOR = colour;
	}
	else {
		COLOR = vec4(0);
	}
}
