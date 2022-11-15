shader_type canvas_item;

uniform vec4 colour : source_color;
uniform vec4 outline_colour : source_color = vec4(0, 0, 0, 1);
uniform float percentage;
uniform float percentage_offset;
uniform float outline_thickness = 0.05;

vec2 polar_coordinates(vec2 uv)
{
	float radius = length(uv);
	float angle = (atan(uv.y, uv.x) / PI + 1.0) / 2.0;
	return vec2(radius, angle);
}

float dist_to_line(vec2 point, vec2 line_end) {
	float a = line_end.y;
	float b = line_end.x;
	
	return abs(a * point.x + b * point.y) / sqrt(a * a + b * b);
}

void fragment() {
	vec2 shiftedUV = (UV - vec2(0.5)) * 2.0;
	
	vec2 polarUV = polar_coordinates(shiftedUV);

	float line1_degrees = (percentage_offset + .25) * PI * 2.0;
	vec2 line1_end = vec2(sin(line1_degrees), cos(line1_degrees));
	float line1_dist = dist_to_line(shiftedUV, line1_end);
	
	float line2_degrees = (percentage_offset + percentage + .25) * PI * 2.0;
	vec2 line2_end = vec2(sin(line2_degrees), cos(line2_degrees));
	float line2_dist = dist_to_line(shiftedUV, line2_end);
	
	if (polarUV.x <= 1.0 && polarUV.y > percentage_offset && polarUV.y < percentage + percentage_offset) {
		if (percentage >= 1.0) {
			if (polarUV.x >= 1.0 - outline_thickness && polarUV.x <= 1.0) {
				COLOR = outline_colour;
			}
			else {
				COLOR = colour;
			}
		}
		else {
			if (polarUV.x >= 1.0 - outline_thickness && polarUV.x <= 1.0) {
				COLOR = outline_colour;
			}
			else if (polarUV.y < percentage_offset + .25 && line1_dist < outline_thickness / 2.0) {
				COLOR = outline_colour;
			}
			else if (polarUV.y > percentage_offset + percentage - .25 && line2_dist < outline_thickness / 2.0) {
				COLOR = outline_colour;
			}
			else if (abs(polarUV.x) < outline_thickness / 2.0) {
				COLOR = outline_colour;
			}
			else {
				COLOR = colour;
			}
		}
	}
	else {
		discard;
	}
}
