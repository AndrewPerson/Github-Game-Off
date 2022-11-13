shader_type canvas_item;

uniform float time_scale = 1;
uniform float dash_count = 3;
uniform float dash_width = 1;
uniform float size = 1;

void fragment() {
	float scaled_dash_count = dash_count * size;
	float scaled_dash_width = dash_width / size;
	
	vec2 adjusted_uv = UV - vec2(0.5);
	
	float opacity = round(mod(adjusted_uv.x * scaled_dash_count - TIME * time_scale, 1.0));
	
	if (adjusted_uv.y > dash_width / -2.0 && adjusted_uv.y < dash_width / 2.0) {
		COLOR = vec4(1, 1, 1, opacity);
	}
	else {
		discard;
	}
}
