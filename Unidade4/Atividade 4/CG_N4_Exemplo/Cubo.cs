//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace gcgcg
{
  internal class Cubo : Objeto
  {
    Ponto4D[] vertices;
    Ponto ponto;
    // int[] indices;
    // Vector3[] normals;
    // int[] colors;

    public Cubo(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.Triangles;
      PrimitivaTamanho = 10;

      vertices = new Ponto4D[]
      {
        new Ponto4D(-1.0f, -1.0f,  1.0f),
        new Ponto4D( 1.0f, -1.0f,  1.0f),
        new Ponto4D( 1.0f,  1.0f,  1.0f),
        new Ponto4D(-1.0f,  1.0f,  1.0f),
        new Ponto4D(-1.0f, -1.0f, -1.0f),
        new Ponto4D( 1.0f, -1.0f, -1.0f),
        new Ponto4D( 1.0f,  1.0f, -1.0f),
        new Ponto4D(-1.0f,  1.0f, -1.0f)
      };

      base.PontosAdicionar(vertices[0]);
      base.PontosAdicionar(vertices[1]);
      base.PontosAdicionar(vertices[2]);
      base.PontosAdicionar(vertices[0]);
      base.PontosAdicionar(vertices[2]);
      base.PontosAdicionar(vertices[3]);

      base.PontosAdicionar(vertices[4]);
      base.PontosAdicionar(vertices[7]);
      base.PontosAdicionar(vertices[6]);
      base.PontosAdicionar(vertices[4]);
      base.PontosAdicionar(vertices[6]);
      base.PontosAdicionar(vertices[5]);

      base.PontosAdicionar(vertices[4]);
      base.PontosAdicionar(vertices[5]);
      base.PontosAdicionar(vertices[0]);
      base.PontosAdicionar(vertices[0]);
      base.PontosAdicionar(vertices[5]);
      base.PontosAdicionar(vertices[1]);

      base.PontosAdicionar(vertices[3]);
      base.PontosAdicionar(vertices[7]);
      base.PontosAdicionar(vertices[4]);
      base.PontosAdicionar(vertices[4]);
      base.PontosAdicionar(vertices[0]);
      base.PontosAdicionar(vertices[3]);

      base.PontosAdicionar(vertices[1]);
      base.PontosAdicionar(vertices[5]);
      base.PontosAdicionar(vertices[2]);
      base.PontosAdicionar(vertices[6]);
      base.PontosAdicionar(vertices[2]);
      base.PontosAdicionar(vertices[5]);

      base.PontosAdicionar(vertices[3]);
      base.PontosAdicionar(vertices[2]);
      base.PontosAdicionar(vertices[6]);
      base.PontosAdicionar(vertices[7]);
      base.PontosAdicionar(vertices[3]);
      base.PontosAdicionar(vertices[6]);

      ponto = new Ponto(this, ref _rotulo, new Ponto4D(2.0, 0.0));
      ponto.PrimitivaTipo = PrimitiveType.Points;
      ponto.PrimitivaTamanho = 15;

      ponto.Atualizar();

      Atualizar();
    }

    private void Atualizar()
    {

      base.ObjetoAtualizar();
    }

    public void RotacionarFilho(float angulo)
    {
      ponto.MatrizRotacao(angulo);
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}
