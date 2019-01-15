	struct NodeTypeArray
	{
		float depth;
		uint  color;
	};
	
	struct NodeTypeArray64
	{
		uint64_t color32_depth32;
	};

	struct NodeTypeSB
	{
		float depth;
		uint  color;
	};

	struct NodeTypeKB_Depth
	{
		float depth;
	};

	struct NodeTypeKB_Color
	{
		uint  color;
	};

	struct NodeTypeLL
	{
		float depth;
		uint  color;
		uint  next;
	};