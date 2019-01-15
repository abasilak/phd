	

vec3 getHeatMap(float value, float minF, float maxF)
	{
        vec3 h_color;
        value = (value-minF)/(maxF-minF);
        value = clamp(value,0,1);

        int NUM_COLORS = 26;
        // column wise
        mat4 color = mat4(
                0,0,1,0,
                0,1,0,0,
                1,1,0,0,
                1,0,0,0
                );
        int idx1;
        int idx2;
        float fractBetween = 0;

        value *= NUM_COLORS-1;
        idx1 = int(floor(value));
        idx2 = idx1+1;
        fractBetween = value - float(idx1);

        h_color.r = (color[idx2][0] - color[idx1][0])*fractBetween + color[idx1][0];
        h_color.g = (color[idx2][1] - color[idx1][1])*fractBetween + color[idx1][1];
        h_color.b = (color[idx2][2] - color[idx1][2])*fractBetween + color[idx1][2];

        return h_color;
	}