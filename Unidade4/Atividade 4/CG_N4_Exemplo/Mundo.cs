#define CG_DEBUG
#define CG_Gizmo      
#define CG_OpenGL      
// #define CG_OpenTK
// #define CG_DirectX      
// #define CG_Privado      

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

//FIXME: padrão Singleton

namespace gcgcg
{
  public class Mundo : GameWindow
  {
    private static Objeto mundo = null;
    private char rotuloNovo = '?';
    private Objeto objetoSelecionado = null;

#if CG_Gizmo
    private readonly float[] _sruEixos =
    {
      -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
       0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
       0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f  /* Z+ */
    };

    private int _vertexBufferObject_sruEixos;
    private int _vertexArrayObject_sruEixos;


    // FPS
    private int frames = 0;
    private Stopwatch stopwatch = new();
#endif

    private readonly float[] _vertices = {
        -1, -1,  1, 0.0f, 0.0f,
         1, -1,  1, 1.0f, 0.0f,
         1,  1,  1, 1.0f, 1.0f,
        -1,  1,  1, 0.0f, 1.0f
    };

    private readonly float[] _vertices2 = {
        -1, -1, -1, 0.0f, 0.0f,
         1, -1, -1, 1.0f, 0.0f,
         1,  1, -1, 1.0f, 1.0f,
        -1,  1, -1, 0.0f, 1.0f
    };

    private readonly float[] _vertices3 = {
        -1, 1,  1, 0.0f, 0.0f,
         1, 1, -1, 1.0f, 1.0f,
         1, 1,  1, 1.0f, 0.0f,
        -1, 1, -1, 0.0f, 1.0f
    };

    private readonly float[] _vertices4 = {
        1, -1, -1, 0.0f, 0.0f,
       -1, -1,  1, 1.0f, 1.0f,
       -1, -1, -1, 1.0f, 0.0f,
        1, -1,  1, 0.0f, 1.0f
    };

    private readonly float[] _vertices5 = {
        1, -1, -1, 0.0f, 0.0f,
        1,  1,  1, 1.0f, 1.0f,
        1, -1,  1, 1.0f, 0.0f,
        1,  1, -1, 0.0f, 1.0f
    };

    private readonly float[] _vertices6 = {
       -1, -1,  1, 0.0f, 0.0f,
       -1,  1, -1, 1.0f, 1.0f,
       -1, -1, -1, 1.0f, 0.0f,
       -1,  1,  1, 0.0f, 1.0f
    };

    private Shader _shaderBranca;
    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;
    private Shader _shaderCiano;
    private Shader _shaderMagenta;
    private Shader _shaderAmarela;

    private Shader _shader;
    private Shader _shader2;
    private Shader _shader3;
    private Shader _shader4;
    private Shader _shader5;
    private Shader _shader6;
    private Texture _texture;
    private Texture _texture2;
    private Texture _texture3;
    private Texture _texture4;
    private Texture _texture5;
    private Texture _texture6;
    private int _vertexBufferObject_texture;
    private int _vertexArrayObject_texture;
    private int _elementBufferObject_texture;
    private int _vertexBufferObject_texture2;
    private int _vertexArrayObject_texture2;
    private int _elementBufferObject_texture2;
    private int _vertexBufferObject_texture3;
    private int _vertexArrayObject_texture3;
    private int _elementBufferObject_texture3;
    private int _vertexBufferObject_texture4;
    private int _vertexArrayObject_texture4;
    private int _elementBufferObject_texture4;
    private int _vertexBufferObject_texture5;
    private int _vertexArrayObject_texture5;
    private int _elementBufferObject_texture5;
    private int _vertexBufferObject_texture6;
    private int _vertexArrayObject_texture6;
    private int _elementBufferObject_texture6;

    private Camera _camera;

    private float yaw = 0f;
    private float pitch = 0f;
    private bool mouseDragging = false;
    private Vector2 lastMouse;

    private List<Ponto4D> verticesCubo;

    private readonly int[] _indexes =
    {
        1, 2, 3,
        0, 1, 3
    };

    private readonly int[] _indexes2 =
    {
        1, 2, 3,
        0, 1, 3
    };

    private readonly int[] _indexes3 =
    {
        3, 0, 1,
        0, 2, 1
    };
    private readonly int[] _indexes4 =
    {
        3, 0, 1,
        0, 2, 1
    };
    private readonly int[] _indexes5 =
    {
        1, 0, 2,
        3, 0, 1
    };
    private readonly int[] _indexes6 =
    {
        1, 0, 2,
        3, 0, 1
    };

    private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);
    private int _vaoModel;
    private Shader _lightingShader;

    private bool light1;
    private bool light2;
    private bool light3;
    private bool light4;
    private bool light5;
    private bool light6;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
           : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo ??= new Objeto(null, ref rotuloNovo); //padrão Singleton
    }


    protected override void OnLoad()
    {
      base.OnLoad();

      Utilitario.Diretivas();
#if CG_DEBUG      
      Console.WriteLine("Tamanho interno da janela de desenho: " + ClientSize.X + "x" + ClientSize.Y);
#endif

      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

      GL.Enable(EnableCap.DepthTest);       // Ativar teste de profundidade
      GL.Enable(EnableCap.CullFace);     // Desenha os dois lados da face
      // GL.FrontFace(FrontFaceDirection.Cw);
      // GL.CullFace(CullFaceMode.FrontAndBack);

      #region Cores
      _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      #endregion

#if CG_Gizmo
      #region Eixos: SRU  
      _vertexBufferObject_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
      GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
      _vertexArrayObject_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);

      stopwatch.Start();
      #endregion
#endif


      #region Objeto: ponto  
      objetoSelecionado = new Ponto(mundo, ref rotuloNovo, new Ponto4D(2.0, 0.0));
      objetoSelecionado.PrimitivaTipo = PrimitiveType.Points;
      objetoSelecionado.PrimitivaTamanho = 5;
      #endregion

      #region Objeto: Cubo
      objetoSelecionado = new Cubo(mundo, ref rotuloNovo);
      #endregion

      #region Front Texture
      verticesCubo = objetoSelecionado.GetPontos();
      GL.Enable(EnableCap.Texture2D);
      _vertexArrayObject_texture = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_texture);

      _vertexBufferObject_texture = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_texture);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

      _elementBufferObject_texture = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject_texture);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indexes.Length * sizeof(uint), _indexes, BufferUsageHint.StaticDraw);

      _shader = new Shader("Shaders/shader_texture.vert", "Shaders/shader_texture.frag");
      _shader.Use();

      var vertexLocation = _shader.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _texture = Texture.LoadFromFile("img/equipe.jpg");
      _texture.Use(TextureUnit.Texture0);

      Retangulo front = new Retangulo(objetoSelecionado, ref rotuloNovo, new Ponto4D(-0.3, -0.3, 0.3), new Ponto4D(0.3, 0.3, 0.3), false);
      front.shaderCor = _shader;
      #endregion

      #region Back Texture
      GL.Enable(EnableCap.Texture2D);
      _vertexArrayObject_texture2 = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_texture2);

      _vertexBufferObject_texture2 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_texture2);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices2.Length * sizeof(float), _vertices2, BufferUsageHint.StaticDraw);

      _elementBufferObject_texture2 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject_texture2);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indexes2.Length * sizeof(uint), _indexes2, BufferUsageHint.StaticDraw);

      _shader2 = new Shader("Shaders/shader_texture.vert", "Shaders/shader_texture.frag");
      _shader2.Use();

      var vertexLocation2 = _shader2.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocation2 = _shader2.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _texture2 = Texture.LoadFromFile("img/equipe.jpg");
      _texture2.Use(TextureUnit.Texture0);

      Retangulo back = new Retangulo(objetoSelecionado, ref rotuloNovo, new Ponto4D(-0.3, -0.3, -0.3), new Ponto4D(0.3, 0.3, -0.3), false);
      back.shaderCor = _shader2;
      #endregion

      #region Top Texture
      GL.Enable(EnableCap.Texture2D);
      _vertexArrayObject_texture3 = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_texture3);

      _vertexBufferObject_texture3 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_texture3);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices3.Length * sizeof(float), _vertices3, BufferUsageHint.StaticDraw);

      _elementBufferObject_texture3 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject_texture3);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indexes3.Length * sizeof(uint), _indexes3, BufferUsageHint.StaticDraw);

      _shader3 = new Shader("Shaders/shader_texture.vert", "Shaders/shader_texture.frag");
      _shader3.Use();

      var vertexLocation3 = _shader3.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocation3 = _shader3.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _texture3 = Texture.LoadFromFile("img/equipe.jpg");
      _texture3.Use(TextureUnit.Texture0);

      Retangulo top = new Retangulo(objetoSelecionado, ref rotuloNovo, new Ponto4D(-0.3, 0.3, -0.3), new Ponto4D(0.3, 0.3, 0.3), false);
      top.shaderCor = _shader3;
      #endregion

      #region Bottom Texture
      GL.Enable(EnableCap.Texture2D);
      _vertexArrayObject_texture4 = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_texture4);

      _vertexBufferObject_texture4 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_texture4);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices4.Length * sizeof(float), _vertices4, BufferUsageHint.StaticDraw);

      _elementBufferObject_texture4 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject_texture4);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indexes4.Length * sizeof(uint), _indexes4, BufferUsageHint.StaticDraw);

      _shader4 = new Shader("Shaders/shader_texture.vert", "Shaders/shader_texture.frag");
      _shader4.Use();

      var vertexLocation4 = _shader4.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocation4 = _shader4.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _texture4 = Texture.LoadFromFile("img/equipe.jpg");
      _texture4.Use(TextureUnit.Texture0);

      Retangulo bottom = new Retangulo(objetoSelecionado, ref rotuloNovo, new Ponto4D(-0.3, -0.3, -0.3), new Ponto4D(0.3, -0.3, 0.3), false);
      bottom.shaderCor = _shader4;

      #endregion

      #region Right Texture
      GL.Enable(EnableCap.Texture2D);
      _vertexArrayObject_texture5 = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_texture5);

      _vertexBufferObject_texture5 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_texture5);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices5.Length * sizeof(float), _vertices5, BufferUsageHint.StaticDraw);

      _elementBufferObject_texture5 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject_texture5);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indexes5.Length * sizeof(uint), _indexes5, BufferUsageHint.StaticDraw);

      _shader5 = new Shader("Shaders/shader_texture.vert", "Shaders/shader_texture.frag");
      _shader5.Use();

      var vertexLocation5 = _shader5.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocation5 = _shader5.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _texture5 = Texture.LoadFromFile("img/equipe.jpg");
      _texture5.Use(TextureUnit.Texture0);

      Retangulo right = new Retangulo(objetoSelecionado, ref rotuloNovo, new Ponto4D(0.3, -0.3, -0.3), new Ponto4D(0.3, 0.3, 0.3), true);
      right.shaderCor = _shader5;
      #endregion

      #region Left Texture
      GL.Enable(EnableCap.Texture2D);
      _vertexArrayObject_texture6 = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_texture6);

      _vertexBufferObject_texture6 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_texture6);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices6.Length * sizeof(float), _vertices6, BufferUsageHint.StaticDraw);

      _elementBufferObject_texture6 = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject_texture5);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indexes6.Length * sizeof(uint), _indexes6, BufferUsageHint.StaticDraw);

      _shader6 = new Shader("Shaders/shader_texture.vert", "Shaders/shader_texture.frag");
      _shader6.Use();

      var vertexLocation6 = _shader6.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocation6 = _shader6.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _texture6 = Texture.LoadFromFile("img/equipe.jpg");
      _texture6.Use(TextureUnit.Texture0);

      Retangulo left = new Retangulo(objetoSelecionado, ref rotuloNovo, new Ponto4D(-0.3, -0.3, -0.3), new Ponto4D(-0.3, 0.3, 0.3), true);
      left.shaderCor = _shader6;
      #endregion

      SelecionaLuz(0);

      _camera = new Camera(Vector3.UnitZ * 5, ClientSize.X / (float)ClientSize.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      mundo.Desenhar(new Transformacao4D(), _camera);

      if (!(light1 || light2 || light3 || light4 || light5 || light6))
      {
        DesenharTextura(_vertexArrayObject_texture, _texture, _shader, _indexes);
        DesenharTextura(_vertexArrayObject_texture2, _texture2, _shader2, _indexes2);
        DesenharTextura(_vertexArrayObject_texture3, _texture3, _shader3, _indexes3);
        DesenharTextura(_vertexArrayObject_texture4, _texture4, _shader4, _indexes4);
        DesenharTextura(_vertexArrayObject_texture5, _texture5, _shader5, _indexes5);
        DesenharTextura(_vertexArrayObject_texture6, _texture6, _shader6, _indexes6);
      }

      if (light1)
        SetLight1();

      if (light2)
        SetLight2();

      if (light3)
        SetLight3();

      if (light4)
        SetLight4();

      if (light5)
        SetLight5();

      if (light6)
        SetLight6();

#if CG_Gizmo
      Gizmo_Sru3D();

      frames++;
      if (stopwatch.ElapsedMilliseconds >= 1000)
      {
        Console.WriteLine($"FPS: {frames}");
        frames = 0;
        stopwatch.Restart();
      }
#endif
      SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      #region Teclado
      var input = KeyboardState;

      if (input.IsKeyDown(Keys.Escape))
        Close();

      if (input.IsKeyPressed(Keys.D1))
        SelecionaLuz(1);

      if (input.IsKeyPressed(Keys.D2))
        SelecionaLuz(2);

      if (input.IsKeyPressed(Keys.D3))
        SelecionaLuz(3);

      if (input.IsKeyPressed(Keys.D4))
        SelecionaLuz(4);

      if (input.IsKeyPressed(Keys.D5))
        SelecionaLuz(5);

      if (input.IsKeyPressed(Keys.D6))
        SelecionaLuz(6);

      if (input.IsKeyPressed(Keys.D0))
        SelecionaLuz(0);

      const float cameraSpeed = 0.5f;

      if (input.IsKeyDown(Keys.Z))
        _camera.Position = Vector3.UnitZ;

      if (input.IsKeyDown(Keys.F))
        _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward

      if (input.IsKeyDown(Keys.T))
        _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards

      if (input.IsKeyDown(Keys.Left))
        _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left

      if (input.IsKeyDown(Keys.Right))
        _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right

      if (input.IsKeyDown(Keys.Up))
        _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up

      if (input.IsKeyDown(Keys.Down))
        _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down

      if (input.IsKeyDown(Keys.H))
        _camera.Pitch += -0.5f;

      if (input.IsKeyDown(Keys.Y))
        _camera.Pitch += 0.5f;

      if (input.IsKeyDown(Keys.G))
        _camera.Yaw += -0.5f;

      if (input.IsKeyDown(Keys.J))
        _camera.Yaw += 0.5f;
      #endregion

      #region  Mouse

      if (MouseState.IsButtonPressed(MouseButton.Left))
      {
        System.Console.WriteLine("MouseState.IsButtonPressed(MouseButton.Left)");
        System.Console.WriteLine("__ Valores do Espaço de Tela");
        System.Console.WriteLine("Vector2 mousePosition: " + MousePosition);
        // System.Console.WriteLine("X: " + MouseState.X +" | PreviousX: "+ MouseState.PreviousX);
        System.Console.WriteLine("Vector2i windowSize: " + Size);
      }
      Vector3 target = Vector3.Zero;
      float distance = 5f;

      if (MouseState.IsButtonDown(MouseButton.Left))
      {
        if (!mouseDragging)
        {
          lastMouse = new Vector2(MousePosition.X, MousePosition.Y);
          mouseDragging = true;
        }
        else
        {
          Vector2 currentMouse = new Vector2(MousePosition.X, MousePosition.Y);
          Vector2 delta = currentMouse - lastMouse;
          lastMouse = currentMouse;

          float sensitivity = 0.3f;
          yaw += delta.X * sensitivity;
          pitch -= delta.Y * sensitivity;
          pitch = Math.Clamp(pitch, -89f, 89f);
        }

        float yawRad = MathHelper.DegreesToRadians(yaw);
        float pitchRad = MathHelper.DegreesToRadians(pitch);

        float x = distance * MathF.Cos(pitchRad) * MathF.Cos(yawRad);
        float y = distance * MathF.Sin(pitchRad);
        float z = distance * MathF.Cos(pitchRad) * MathF.Sin(yawRad);

        _camera.Position = new Vector3(x, y, z) + target;

        Vector3 direction = Vector3.Normalize(target - _camera.Position);
        _camera.Pitch = MathHelper.RadiansToDegrees(MathF.Asin(direction.Y));
        _camera.Yaw = MathHelper.RadiansToDegrees(MathF.Atan2(direction.Z, direction.X));
      }
      else
      {
        mouseDragging = false;
      }

      #endregion

    }

    private void SelecionaLuz(int numero)
    {
      light1 = false;
      light2 = false;
      light3 = false;
      light4 = false;
      light5 = false;
      light6 = false;

      switch (numero)
      {

        case 1:
          _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
          light1 = true;
          break;

        case 2:
          _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting2.frag");
          light2 = true;
          break;

        case 3:
          _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting3.frag");
          light3 = true;
          break;

        case 4:
          _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting4.frag");
          light4 = true;
          break;

        case 5:
          _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting5.frag");
          light5 = true;
          break;

        case 6:
          _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting6.frag");
          light6 = true;
          break;

        default:
          _lightingShader = _shader6;
          objetoSelecionado.shaderCor = _lightingShader;
          // mundo.GrafocenaBuscaProximo(objetoSelecionado).shaderCor = _lightingShader;
          break;
      }

      {
        _vaoModel = GL.GenVertexArray();
        GL.BindVertexArray(_vaoModel);

        var positionLocation = _lightingShader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        var normalLocation = _lightingShader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

        var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
      }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);

#if CG_DEBUG      
      Console.WriteLine("Tamanho interno da janela de desenho: " + ClientSize.X + "x" + ClientSize.Y);
#endif
      GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
    }

    protected override void OnUnload()
    {
      mundo.OnUnload();

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);

      GL.DeleteBuffer(_vertexBufferObject_sruEixos);
      GL.DeleteVertexArray(_vertexArrayObject_sruEixos);

      GL.DeleteProgram(_shaderBranca.Handle);
      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);
      GL.DeleteProgram(_shaderCiano.Handle);
      GL.DeleteProgram(_shaderMagenta.Handle);
      GL.DeleteProgram(_shaderAmarela.Handle);

      base.OnUnload();
    }

#if CG_Gizmo
    private void Gizmo_Sru3D()
    {
#if CG_OpenGL && !CG_DirectX
      var model = Matrix4.Identity;
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      // EixoX
      _shaderVermelha.SetMatrix4("model", model);
      _shaderVermelha.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderVermelha.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderVermelha.Use();
      GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      // EixoY
      _shaderVerde.SetMatrix4("model", model);
      _shaderVerde.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderVerde.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderVerde.Use();
      GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      // EixoZ
      _shaderAzul.SetMatrix4("model", model);
      _shaderAzul.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderAzul.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderAzul.Use();
      GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
    }
#endif

    private void SetLight1()
    {
      GL.BindVertexArray(_vaoModel);
      _lightingShader.Use();

      _lightingShader.SetMatrix4("model", Matrix4.Identity);
      _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      _lightingShader.SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
      _lightingShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
      _lightingShader.SetVector3("lightPos", _lightPos);
      _lightingShader.SetVector3("viewPos", _camera.Position);

      objetoSelecionado.shaderCor = _lightingShader;
      mundo.GrafocenaBuscaProximo(objetoSelecionado).shaderCor = _lightingShader;
    }

    private void SetLight2()
    {
      GL.BindVertexArray(_vaoModel);
      _lightingShader.Use();

      _lightingShader.SetMatrix4("model", Matrix4.Identity);
      _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      _lightingShader.SetVector3("viewPos", _camera.Position);

      _lightingShader.SetInt("material.diffuse", 0);
      _lightingShader.SetInt("material.specular", 1);
      _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
      _lightingShader.SetFloat("material.shininess", 32.0f);

      _lightingShader.SetVector3("light.position", _lightPos);
      _lightingShader.SetVector3("light.ambient", new Vector3(2.0f));
      _lightingShader.SetVector3("light.diffuse", new Vector3(1f));
      _lightingShader.SetVector3("light.specular", new Vector3(1.0f));

      objetoSelecionado.shaderCor = _lightingShader;
      mundo.GrafocenaBuscaProximo(objetoSelecionado).shaderCor = _lightingShader;
    }

    private void SetLight3()
    {
      GL.BindVertexArray(_vaoModel);
      _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      _lightingShader.SetInt("material.diffuse", 0);
      _lightingShader.SetInt("material.specular", 1);
      _lightingShader.SetFloat("material.shininess", 32.0f);

      _lightingShader.SetVector3("viewPos", _camera.Position);
      _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
      _lightingShader.SetVector3("light.direction", new Vector3(-0.2f, -1.0f, -0.3f));
      _lightingShader.SetVector3("light.ambient", new Vector3(0.2f));
      _lightingShader.SetVector3("light.diffuse", new Vector3(0.5f));
      _lightingShader.SetVector3("light.specular", new Vector3(1.0f));

      objetoSelecionado.shaderCor = _lightingShader;
      mundo.GrafocenaBuscaProximo(objetoSelecionado).shaderCor = _lightingShader;
    }

    private void SetLight4()
    {
      GL.BindVertexArray(_vaoModel);
      _lightingShader.Use();

      _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      _lightingShader.SetVector3("viewPos", _camera.Position);

      _lightingShader.SetInt("material.diffuse", 0);
      _lightingShader.SetInt("material.specular", 1);
      _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
      _lightingShader.SetFloat("material.shininess", 32.0f);

      _lightingShader.SetVector3("light.position", _lightPos);
      _lightingShader.SetFloat("light.constant", 1.0f);
      _lightingShader.SetFloat("light.linear", 0.09f);
      _lightingShader.SetFloat("light.quadratic", 0.032f);
      _lightingShader.SetVector3("light.ambient", new Vector3(0.2f));
      _lightingShader.SetVector3("light.diffuse", new Vector3(0.5f));
      _lightingShader.SetVector3("light.specular", new Vector3(1.0f));

      objetoSelecionado.shaderCor = _lightingShader;
      mundo.GrafocenaBuscaProximo(objetoSelecionado).shaderCor = _lightingShader;
    }

    private void SetLight5()
    {
      GL.BindVertexArray(_vaoModel);
      _lightingShader.Use();

      _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      _lightingShader.SetVector3("viewPos", _camera.Position);

      _lightingShader.SetInt("material.diffuse", 0);
      _lightingShader.SetInt("material.specular", 1);
      _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
      _lightingShader.SetFloat("material.shininess", 32.0f);

      _lightingShader.SetVector3("light.position", _camera.Position);
      _lightingShader.SetVector3("light.direction", _camera.Front);
      _lightingShader.SetFloat("light.cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
      _lightingShader.SetFloat("light.outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(17.5f)));
      _lightingShader.SetFloat("light.constant", 1.0f);
      _lightingShader.SetFloat("light.linear", 0.09f);
      _lightingShader.SetFloat("light.quadratic", 0.032f);
      _lightingShader.SetVector3("light.ambient", new Vector3(0.2f));
      _lightingShader.SetVector3("light.diffuse", new Vector3(0.5f));
      _lightingShader.SetVector3("light.specular", new Vector3(1.0f));

      objetoSelecionado.shaderCor = _lightingShader;
      mundo.GrafocenaBuscaProximo(objetoSelecionado).shaderCor = _lightingShader;
    }

    private void SetLight6()
    {
      GL.BindVertexArray(_vaoModel);
      _lightingShader.Use();

      _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      _lightingShader.SetVector3("viewPos", _camera.Position);

      _lightingShader.SetInt("material.diffuse", 0);
      _lightingShader.SetInt("material.specular", 1);
      _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
      _lightingShader.SetFloat("material.shininess", 32.0f);

      _lightingShader.SetVector3("dirLight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
      _lightingShader.SetVector3("dirLight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
      _lightingShader.SetVector3("dirLight.diffuse", new Vector3(0.4f, 0.4f, 0.4f));
      _lightingShader.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));

      _lightingShader.SetVector3("spotLight.position", _camera.Position);
      _lightingShader.SetVector3("spotLight.direction", _camera.Front);
      _lightingShader.SetVector3("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
      _lightingShader.SetVector3("spotLight.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
      _lightingShader.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
      _lightingShader.SetFloat("spotLight.constant", 1.0f);
      _lightingShader.SetFloat("spotLight.linear", 0.09f);
      _lightingShader.SetFloat("spotLight.quadratic", 0.032f);
      _lightingShader.SetFloat("spotLight.cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
      _lightingShader.SetFloat("spotLight.outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(17.5f)));

      objetoSelecionado.shaderCor = _lightingShader;
      mundo.GrafocenaBuscaProximo(objetoSelecionado).shaderCor = _lightingShader;
    }

    private void DesenharTextura(int vertexArrayObject, Texture texture, Shader shader, int[] indices)
    {
      GL.BindVertexArray(vertexArrayObject);
      texture.Use(TextureUnit.Texture0);
      shader.Use();
      GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
    }

  }
}
