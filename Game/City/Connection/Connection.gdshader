shader_type canvas_item;

uniform float time_scale = 1;
uniform float dash_count = 10;
uniform float dash_width = 0.05;

uniform vec2 size = vec2(1);

void fragment() {
	float rotation = atan(size.y / size.x);

	vec2 adjusted_uv = UV - vec2(0.5);
	float x = adjusted_uv.x;
	float y = adjusted_uv.y;
	
	adjusted_uv.x = x * cos(rotation) - y * sin(rotation);
	adjusted_uv.y = x * sin(rotation) + y * cos(rotation);
	
	float opacity = round(mod(adjusted_uv.x * dash_count - TIME * time_scale, 1.0));
	
	if (adjusted_uv.y > dash_width / -2.0 && adjusted_uv.y < dash_width / 2.0)
	{
		COLOR = vec4(1, 1, 1, opacity);
	}
	else
	{
		COLOR = vec4(0);
	}
}
