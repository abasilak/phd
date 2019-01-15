//#if INTEL_ordering
//#version 430 core
//#else
#version 450 core
#extension GL_NV_gpu_shader5  : enable
//#endif

#define INTEL_ordering		0
#define NV_interlock		0

#define early_Z				1
#define early_clipping		1
#define early_clipping_rest 0
#define multisample			0
#define packing				0
#define multipass			0
#define random_discard		0
//#define peeling_error		0

#define ARRAY_SIZE			200 // { AB_PRECALC_FIXED }

#define HEAP_SIZE			8	// { KB_ALL } 
#define HEAP_SIZE_1p		HEAP_SIZE + 1
#define HEAP_SIZE_1n		HEAP_SIZE - 1
#define HEAP_SIZE_2d		HEAP_SIZE >> 1
#define HEAP_SIZE_LOG2		log2(HEAP_SIZE)
#define ARRAY_VS_HEAP		16
#define INSERTION_VS_SHELL	16

#define KB_SIZE				8
#define STENCIL_SIZE		((HEAP_SIZE < 32) ? HEAP_SIZE : 32)

#define MAX_ITERATIONS		1000

// { CLIPPING }
#define BUCKET_SIZE_32d		((HEAP_SIZE > 32) ? HEAP_SIZE : 32) 
#define BUCKET_SIZE			BUCKET_SIZE_32d * 32
#define	BUCKET_SIZE_div		1.0f / float(BUCKET_SIZE)

// { PAGING }
#define PAGE_SIZE			4	// { Linked_Lists_Paged }
#define BUN_SIZE			8	// { Linked_Lists_BUN }
#define HISTOGRAM_SIZE		1024
#define LOCAL_SIZE			32	// { RESOLVE }
#define LOCAL_SIZE_1n		LOCAL_SIZE - 1

// Applications
#define trimless			0	// { FreePipe, ...}
#define trimming			0	// { FreePipe, ...}
#define collision			0	// { FreePipe, ...}

#define Packed_1f	4294967295U // 0xFFFFFFFFU
#define depth_max	1.0f

//precision highp float;
//precision highp int;

/*
#define multisample 0			// All except { KB_Stencil }
#define packing		0			// { F2B_KB, DUAL_KB_1B, DUAL_KB_2B, KB, MultiKB, HT_1 }
#define a_buffer	1			// { F2B_FreePipe, F2B_LL, DUAL_FreePipe, DUAL_LL, FreePipe, Linked_Lists, S-Buffer }

//#if a_buffer
#version 430 core
#extension GL_NV_gpu_shader5 : enable
//#else
//#version 330 core
//#extension GL_ARB_shading_language_420pack : enable
//#endif


#define peeling_error		0	// { F2B, F2B2, F2B_3P_MAX, F2B_3P_MIN_MAX}

#define multipass			0	// { KB, MultiKB, K_STENCIL, BUN, FreePipe, KB_HEAP}

#define draw_buffers_blend	1	// { F2B_3P_MAX, F2B_3P_MIN_MAX, F2B_2P_MAX, F2B_2P_MIN_MAX,
								//   DUAL_3P_MAX, DUAL_3P_MIN_MAX, DUAL_2P_MAX, DUAL_2P_MIN_MAX }

#define sorted_by_id		0	// { F2B_KB, F2B_FreePipe, F2B_LL, 
								//   DUAL_KB_1B, DUAL_KB_2B, DUAL_FreePipe, DUAL_LL,
								//   MultiKB_Z }

#define STENCIL_SIZE		32	// { KB_Stencil }
#define KB_SIZE				8	// { F2B_KB, DUAL_KB_1B, KB, MultiKB, K_STENCIL }

#define FREEPIPE_SIZE		150 // { F2B_FreePipe, DUAL_FreePipe, FreePipe, BUCKET_LL }

#define BUCKET_SIZE			4	// { BUCKET_LL }
#define PAGE_SIZE			4	// { Linked_Lists_Paged }

#define depth_max	1.0f
#define float_max   3.40282e+038f
#define float_min  -3.40282e+038f
*/
