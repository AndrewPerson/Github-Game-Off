shader_type canvas_item;

void fragment() {
	if (distance(vec2(0.5), UV) > 0.5) {
		discard;
	}
}
