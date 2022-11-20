shader_type canvas_item;

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

    float shifted_percentage = percentage_offset + percentage;

    float line1_angle_dist = min(abs(1.0 + percentage_offset - polarUV.y), abs(polarUV.y - percentage_offset));
    float line2_angle_dist = min(abs(1.0 + shifted_percentage - polarUV.y), abs(polarUV.y - shifted_percentage));

    if (polarUV.x <= 1.0) {
        if (percentage < 1.0) {
            if (line1_dist < outline_thickness / 2.0 && line1_angle_dist <= .25) {
                COLOR = vec4(0, 0, 0, 1);
            }
            else if (line2_dist < outline_thickness / 2.0 && line2_angle_dist <= .25) {
                COLOR = vec4(0, 0, 0, 1);
            }
            else if (1.0 - polarUV.x <= outline_thickness && polarUV.y >= percentage_offset && polarUV.y <= percentage + percentage_offset) {
                COLOR = vec4(0, 0, 0, 1);
            }
            else if (polarUV.x <= outline_thickness / 2.0) {
                COLOR = vec4(0, 0, 0, 1);
            }
            else if (!(polarUV.y >= percentage_offset && polarUV.y <= percentage + percentage_offset)) {
                discard;
            }
        }
        else {
            if (1.0 - polarUV.x <= outline_thickness) {
                COLOR = vec4(0, 0, 0, 1);
            }
            else if (polarUV.x <= outline_thickness / 2.0) {
                COLOR = vec4(0, 0, 0, 1);
            }
            else if (!(polarUV.y >= percentage_offset && polarUV.y <= percentage + percentage_offset)) {
                discard;
            }
        }
    }
    else {
        discard;
    }

    /*
    if (polarUV.x <= 1.0 && polarUV.y > percentage_offset && polarUV.y < percentage + percentage_offset) {
        if (percentage >= 1.0) {
            if (polarUV.x >= 1.0 - outline_thickness && polarUV.x <= 1.0) {
                COLOR = vec4(0, 0, 0, 1);
            }
        }
        else {
            if (polarUV.x >= 1.0 - outline_thickness && polarUV.x <= 1.0) {
                COLOR = vec4(0, 0, 0, 1);
            }
            else if (polarUV.y < percentage_offset + .25 && line1_dist < outline_thickness) {
                COLOR = vec4(0, 0, 0, 1);
            }
            else if (polarUV.y > percentage_offset + percentage - .25 && line2_dist < outline_thickness) {
                COLOR = vec4(0, 0, 0, 1);
            }
            else if (abs(polarUV.x) < outline_thickness) {
                COLOR = vec4(0, 0, 0, 1);
            }
        }
    }
    else {
        discard;
    }
    */
}
