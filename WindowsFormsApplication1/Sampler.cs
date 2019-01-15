using OpenTK.Graphics.OpenGL;

namespace abasilak
{
    /// <summary>
    /// Sampler
    /// </summary>
    public class Sampler
    {
        uint _index;

        public Sampler()
        {
            GL.GenSamplers(1, out _index);

            GL.SamplerParameter(_index, SamplerParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.SamplerParameter(_index, SamplerParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.SamplerParameter(_index, SamplerParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
            GL.SamplerParameter(_index, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.SamplerParameter(_index, SamplerParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            GL.SamplerParameter(_index, SamplerParameterName.TextureMinLod, -1000.0f);
            GL.SamplerParameter(_index, SamplerParameterName.TextureMaxLod, 1000.0f);
            GL.SamplerParameter(_index, SamplerParameterName.TextureLodBias, 0.0f);
            GL.SamplerParameter(_index, SamplerParameterName.TextureBorderColor, new float[] { 1, 0, 0, 0 });

            GL.SamplerParameter(_index, SamplerParameterName.TextureCompareMode, (int)All.None);
            GL.SamplerParameter(_index, SamplerParameterName.TextureCompareFunc, (int)All.Lequal);

            //GL.SamplerParameterI(_index, SamplerParameterName.TextureMaxAnisotropyExt, ref ani);
          
        }
        public void bind(uint tex_index)
        {
            GL.BindSampler(tex_index, _index);
        }
        public void delete()
        {
            GL.DeleteSamplers(1, ref _index);
        }
    }
}