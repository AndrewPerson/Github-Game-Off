shader_type canvas_item;

uniform float time_scale = 1;
uniform float dash_count = 3;
uniform float dash_width = 1;
uniform vec2 size = vec2(100.0, 100.0);

vec2 polar_coordinates(vec2 uv)
{
    float radius = length(uv);
    float angle = (atan(uv.y, uv.x) / PI + 1.0) / 2.0;
    return vec2(radius, angle);
}

void fragment() {
    vec2 circle_point = vec2(size.x / 2.0, size.y);
    vec2 circle_center = vec2(size.x / 2.0, (pow(circle_point.x, 2) + pow(circle_point.y, 2)) / (2.0 * circle_point.y));
    
    vec2 adjusted_uv = UV * size;
    vec2 polar_uv = polar_coordinates(adjusted_uv - circle_center);
    
    float dist = circle_center.y - distance(circle_center, adjusted_uv);
    
    if (dist > 0.0 && dist < dash_width) {
        float opacity = round(mod(polar_uv.y * dash_count * circle_center.y * 2.0 - TIME * time_scale, 1.0));
        COLOR = vec4(1.0, 1.0, 1.0, opacity);
    }
    else {
        discard;
    }

    //Now what?

    /*
    float scaled_dash_count = dash_count * size.x / 100.0;
    float scaled_dash_width = dash_width / size.x / 100.0;
    
    vec2 adjusted_uv = UV - vec2(0.5);
    
    float opacity = round(mod(adjusted_uv.x * scaled_dash_count - TIME * time_scale, 1.0));
    
    if (adjusted_uv.y > dash_width / -2.0 && adjusted_uv.y < dash_width / 2.0) {
        COLOR = vec4(1, 1, 1, opacity);
    }
    else {
        discard;
    }
    */
}
