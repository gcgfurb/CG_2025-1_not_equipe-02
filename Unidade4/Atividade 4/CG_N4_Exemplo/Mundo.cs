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
using System.Collections.Generic;


namespace gcgcg
{
  public class Mundo : GameWindow
  {
    private static Objeto mundo = null;
    private char rotuloNovo = '?';
    private Objeto objetoSelecionado = null;
    private Objeto cubo = null;

    private readonly float[] _sruEixos =
    {
      -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
       0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
       0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f  /* Z+ */
    };

    private int _vertexBufferObjectFront_sruEixos;
    private int _vertexArrayObjectFront_sruEixos;

    private Shader _shaderBranca;
    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;
    private Shader _shaderCiano;
    private Shader _shaderMagenta;
    private Shader _shaderAmarela;

    // Vértices corrigidos com Posição, Normal e Coordenadas de Textura (8 floats por vértice)
    private readonly float[] _vertices =
    {
        // Posição              Normal              TexCoord
        // Face frontal (Z-)
        -1.05f, -1.05f, -1.05f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
         1.05f, -1.05f, -1.05f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
         1.05f,  1.05f, -1.05f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
         1.05f,  1.05f, -1.05f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
        -1.05f,  1.05f, -1.05f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
        -1.05f, -1.05f, -1.05f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

        // Face traseira (Z+)
        -1.05f, -1.05f,  1.05f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
         1.05f, -1.05f,  1.05f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
         1.05f,  1.05f,  1.05f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
         1.05f,  1.05f,  1.05f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
        -1.05f,  1.05f,  1.05f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
        -1.05f, -1.05f,  1.05f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,

        // Face esquerda (X-)
        -1.05f,  1.05f,  1.05f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
        -1.05f,  1.05f, -1.05f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
        -1.05f, -1.05f, -1.05f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
        -1.05f, -1.05f, -1.05f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
        -1.05f, -1.05f,  1.05f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
        -1.05f,  1.05f,  1.05f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,

        // Face direita (X+)
         1.05f,  1.05f,  1.05f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
         1.05f,  1.05f, -1.05f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
         1.05f, -1.05f, -1.05f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
         1.05f, -1.05f, -1.05f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
         1.05f, -1.05f,  1.05f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
         1.05f,  1.05f,  1.05f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,

        // Face inferior (Y-)
        -1.05f, -1.05f, -1.05f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
         1.05f, -1.05f, -1.05f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
         1.05f, -1.05f,  1.05f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
         1.05f, -1.05f,  1.05f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
        -1.05f, -1.05f,  1.05f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
        -1.05f, -1.05f, -1.05f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,

        // Face superior (Y+)
        -1.05f,  1.05f, -1.05f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
         1.05f,  1.05f, -1.05f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
         1.05f,  1.05f,  1.05f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
         1.05f,  1.05f,  1.05f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
        -1.05f,  1.05f,  1.05f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
        -1.05f,  1.05f, -1.05f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f
    };

    private readonly uint[] _indices =
    {
      0, 1, 3, 1, 2, 3
    };

    private readonly Vector3 _lightPos = new Vector3(2.0f, 2.5f, 2.0f);

    // We need the point lights' positions to draw the lamps and to get light the materials properly
    private readonly Vector3[] _pointLightPositions =
    {
        new Vector3(0.7f, 0.2f, 2.0f),
        new Vector3(2.3f, -3.3f, -4.0f),
        new Vector3(-4.0f, 2.0f, -12.0f),
        new Vector3(0.0f, 0.0f, -3.0f)
    };

    private int _vertexBufferObject;

    private int _elementBufferObject;

    private int _vertexArrayObject;

    private int _vaoModel;

    private int _vaoLamp;

    private Shader _lampShader;

    private Shader _lightingShader;

    private Shader _shader;

    private Shader _basicLighting;
    private Shader _lightingMaps;
    private Shader _directionalLights;
    private Shader _pointLights;
    private Shader _spotlight;
    private Shader _multipleLights;

    private Shader _currentShader;

    private Texture _texture;
    private Texture _diffuseMap;
    private Texture _specularMap;

    private readonly Vector3[] _cubePositions =
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(-10.5f, -12.2f, -2.5f),
      new Vector3(-13.8f, -12.0f, -12.3f),
      new Vector3(12.4f, -20.4f, -3.5f),
      new Vector3(-15.7f, 13.0f, -7.5f),
      new Vector3(15.3f, -6.0f, -2.5f),
      new Vector3(19.5f, 21.0f, -2.5f),
      new Vector3(11.5f, 30.2f, -1.5f),
      new Vector3(-11.3f, 13.0f, -1.5f)
    };

    private Camera _camera;
    private Vector3 _origin = new(0, 0, 0);

    private bool mouseDragging = false;
    private Vector2 lastMouse;
    private float yaw;
    private float pitch;

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

      GL.Enable(EnableCap.DepthTest);

      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

      _shader = new Shader("Shaders/shaderTexture.vert", "Shaders/shaderTexture.frag");
      _shader.Use();

      _currentShader = _shader;

      #region Iluminação
      _basicLighting = new Shader("Shaders/lighting.vert", "Shaders/lighting.frag");
      _lightingMaps = new Shader("Shaders/lmaps.vert", "Shaders/lmaps.frag");
      _directionalLights = new Shader("Shaders/dlights.vert", "Shaders/dlights.frag");
      _pointLights = new Shader("Shaders/plights.vert", "Shaders/plights.frag");
      _spotlight = new Shader("Shaders/spotlight.vert", "Shaders/spotlight.frag");
      _multipleLights = new Shader("Shaders/mlights.vert", "Shaders/mlights.frag");
      #endregion

      #region Cores
      _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      #endregion

      #region Eixos: SRU  
      _vertexBufferObjectFront_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectFront_sruEixos);
      GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
      _vertexArrayObjectFront_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObjectFront_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);
      #endregion

      GL.Enable(EnableCap.DepthTest);

      _vertexArrayObject = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject);

      _vertexBufferObject = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

      // Configuração corrigida dos atributos de vértice
      var vertexLocation = _shader.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

      // Atributo normal (se necessário)
      var normalLocation = _shader.GetAttribLocation("aNormal");
      if (normalLocation >= 0)
      {
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
      }

      // Coordenadas de textura corrigidas
      var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

      _elementBufferObject = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

      _lightingShader = new Shader("Shaders/lighting.vert", "Shaders/lighting.frag");
      _lampShader = new Shader("Shaders/shaderLamp.vert", "Shaders/shaderLamp.frag");

      _texture = Texture.LoadFromFile("Resources/equipe.png");
      _texture.Use(TextureUnit.Texture0);

      _shader.SetInt("texture0", 0);

      _diffuseMap = Texture.LoadFromFile("Resources/equipe.png");
      _specularMap = Texture.LoadFromFile("Resources/specular.png");

      {
        _vaoModel = GL.GenVertexArray();
        GL.BindVertexArray(_vaoModel);

        var positionLocation = _lightingShader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        var normalLocation2 = _lightingShader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation2);
        GL.VertexAttribPointer(normalLocation2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
      }

      {
        _vaoLamp = GL.GenVertexArray();
        GL.BindVertexArray(_vaoLamp);

        var positionLocation = _lampShader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
      }

      #region Objeto: Cubo
      objetoSelecionado = new Cubo(mundo, ref rotuloNovo);
      cubo = objetoSelecionado;
      objetoSelecionado.shaderCor = _shaderAmarela;
      #endregion

      _camera = new Camera(Vector3.UnitZ * 5, ClientSize.X / (float)ClientSize.Y);

      Vector3 dir = Vector3.Normalize(_camera.Position);
      pitch = MathHelper.RadiansToDegrees(MathF.Asin(dir.Y));
      yaw = MathHelper.RadiansToDegrees(MathF.Atan2(dir.Z, dir.X));
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      mundo.Desenhar(new Transformacao4D(), _camera);

      GL.BindVertexArray(_vaoModel);

      if (_currentShader == _basicLighting)
      {
        _texture.Use(TextureUnit.Texture0);
        _currentShader.Use();

        _lightingShader.Use();

        _lightingShader.SetMatrix4("model", Matrix4.Identity);
        _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _lightingShader.SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
        _lightingShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
        _lightingShader.SetVector3("lightPos", _lightPos);
        _lightingShader.SetVector3("viewPos", _camera.Position);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

        GL.BindVertexArray(_vaoLamp);

        _lampShader.Use();

        Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
        lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

        _lampShader.SetMatrix4("model", lampMatrix);
        _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }

      if (_currentShader == _lightingMaps)
      {
        // The two textures need to be used, in this case we use the diffuse map as our 0th texture
        // and the specular map as our 1st texture.
        _specularMap.Use(TextureUnit.Texture0);
        _diffuseMap.Use(TextureUnit.Texture1);

        _lightingMaps.SetInt("material.specular", 0);
        _lightingMaps.SetInt("material.diffuse", 1);

        _lightingShader.Use();
        _lightingMaps.Use();

        _lightingMaps.SetMatrix4("model", Matrix4.Identity);
        _lightingMaps.SetMatrix4("view", _camera.GetViewMatrix());
        _lightingMaps.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _lightingMaps.SetVector3("viewPos", _camera.Position);

        // Here we specify to the shaders what textures they should refer to when we want to get the positions.
        _lightingMaps.SetInt("material.diffuse", 0);
        _lightingMaps.SetInt("material.specular", 1);
        _lightingMaps.SetFloat("material.shininess", 12.0f);

        _lightingMaps.SetVector3("light.position", _lightPos);
        _lightingMaps.SetVector3("light.ambient", new Vector3(0.5f));
        _lightingMaps.SetVector3("light.diffuse", new Vector3(1.0f));
        _lightingMaps.SetVector3("light.specular", new Vector3(2.0f));

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

        GL.BindVertexArray(_vaoLamp);

        _lampShader.Use();

        Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
        lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

        _lampShader.SetMatrix4("model", lampMatrix);
        _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }

      if (_currentShader == _directionalLights)
      {
        _diffuseMap.Use(TextureUnit.Texture0);
        _specularMap.Use(TextureUnit.Texture1);
        _directionalLights.Use();

        _directionalLights.SetMatrix4("view", _camera.GetViewMatrix());
        _directionalLights.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _directionalLights.SetVector3("viewPos", _camera.Position);

        _directionalLights.SetInt("material.diffuse", 0);
        _directionalLights.SetInt("material.specular", 1);
        _directionalLights.SetFloat("material.shininess", 32.0f);

        // Directional light needs a direction, in this example we just use (-0.2, -1.0, -0.3f) as the lights direction
        _directionalLights.SetVector3("light.direction", new Vector3(-0.2f, -1.0f, -0.3f));
        _directionalLights.SetVector3("light.ambient", new Vector3(0.4f));
        _directionalLights.SetVector3("light.diffuse", new Vector3(1.0f));
        _directionalLights.SetVector3("light.specular", new Vector3(1.0f));

        // We want to draw all the cubes at their respective positions
        for (int i = 0; i < _cubePositions.Length; i++)
        {
          // Then we translate said matrix by the cube position
          Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
          // We then calculate the angle and rotate the model around an axis
          float angle = 20.0f * i;
          model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
          // Remember to set the model at last so it can be used by opentk
          _directionalLights.SetMatrix4("model", model);

          // At last we draw all our cubes
          GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        GL.BindVertexArray(_vaoLamp);

        _lampShader.Use();

        Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
        lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

        _lampShader.SetMatrix4("model", lampMatrix);
        _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }

      if (_currentShader == _pointLights)
      {
        _diffuseMap.Use(TextureUnit.Texture0);
        _specularMap.Use(TextureUnit.Texture1);
        _pointLights.Use();

        _pointLights.SetMatrix4("view", _camera.GetViewMatrix());
        _pointLights.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _pointLights.SetVector3("viewPos", _camera.Position);

        _pointLights.SetInt("material.diffuse", 0);
        _pointLights.SetInt("material.specular", 1);
        _pointLights.SetFloat("material.shininess", 32.0f);

        _pointLights.SetVector3("light.position", _lightPos);
        _pointLights.SetFloat("light.constant", 1.0f);
        _pointLights.SetFloat("light.linear", 0.09f);
        _pointLights.SetFloat("light.quadratic", 0.032f);
        _pointLights.SetVector3("light.ambient", new Vector3(5.0f));
        _pointLights.SetVector3("light.diffuse", new Vector3(0.5f));
        _pointLights.SetVector3("light.specular", new Vector3(1.0f));

        // We want to draw all the cubes at their respective positions
        for (int i = 0; i < _cubePositions.Length; i++)
        {
          // First we create a model from an identity matrix
          // Then we translate said matrix by the cube position
          Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
          // We then calculate the angle and rotate the model around an axis
          float angle = 20.0f * i;
          model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
          // Remember to set the model at last so it can be used by opentk
          _pointLights.SetMatrix4("model", model);

          // At last we draw all our cubes
          GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        GL.BindVertexArray(_vaoLamp);

        _lampShader.Use();

        Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
        lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

        _lampShader.SetMatrix4("model", lampMatrix);
        _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }

      if (_currentShader == _spotlight)
      {

        _diffuseMap.Use(TextureUnit.Texture0);
        _specularMap.Use(TextureUnit.Texture1);

        _spotlight.Use();

        _spotlight.SetMatrix4("view", _camera.GetViewMatrix());
        _spotlight.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _spotlight.SetVector3("viewPos", _camera.Position);

        _spotlight.SetInt("material.diffuse", 0);
        _spotlight.SetInt("material.specular", 1);
        _spotlight.SetFloat("material.shininess", 32.0f);

        _spotlight.SetVector3("light.position", _camera.Position);
        _spotlight.SetVector3("light.direction", _camera.Front);
        _spotlight.SetFloat("light.cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
        _spotlight.SetFloat("light.outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(17.5f)));
        _spotlight.SetFloat("light.constant", 1.0f);
        _spotlight.SetFloat("light.linear", 0.09f);
        _spotlight.SetFloat("light.quadratic", 0.032f);
        _spotlight.SetVector3("light.ambient", new Vector3(0.2f));
        _spotlight.SetVector3("light.diffuse", new Vector3(0.5f));
        _spotlight.SetVector3("light.specular", new Vector3(1.0f));

        // We want to draw all the cubes at their respective positions
        for (int i = 0; i < _cubePositions.Length; i++)
        {
          // Then we translate said matrix by the cube position
          Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
          // We then calculate the angle and rotate the model around an axis
          float angle = 20.0f * i;
          model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
          // Remember to set the model at last so it can be used by opentk
          _spotlight.SetMatrix4("model", model);

          // At last we draw all our cubes
          GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        GL.BindVertexArray(_vaoLamp);

        _lampShader.Use();

        Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
        lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

        _lampShader.SetMatrix4("model", lampMatrix);
        _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }

      if (_currentShader == _multipleLights)
      {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.BindVertexArray(_vaoModel);

        _diffuseMap.Use(TextureUnit.Texture0);
        _specularMap.Use(TextureUnit.Texture1);
        _multipleLights.Use();

        _multipleLights.SetMatrix4("view", _camera.GetViewMatrix());
        _multipleLights.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _multipleLights.SetVector3("viewPos", _camera.Position);

        _multipleLights.SetInt("material.diffuse", 0);
        _multipleLights.SetInt("material.specular", 1);
        _multipleLights.SetFloat("material.shininess", 32.0f);

        /*
           Here we set all the uniforms for the 5/6 types of lights we have. We have to set them manually and index
           the proper PointLight struct in the array to set each uniform variable. This can be done more code-friendly
           by defining light types as classes and set their values in there, or by using a more efficient uniform approach
           by using 'Uniform buffer objects', but that is something we'll discuss in the 'Advanced GLSL' tutorial.
        */
        // Directional light
        _multipleLights.SetVector3("dirLight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
        _multipleLights.SetVector3("dirLight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
        _multipleLights.SetVector3("dirLight.diffuse", new Vector3(0.4f, 0.4f, 0.4f));
        _multipleLights.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));

        // Point lights
        for (int i = 0; i < _pointLightPositions.Length; i++)
        {
          _multipleLights.SetVector3($"pointLights[{i}].position", _pointLightPositions[i]);
          _multipleLights.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
          _multipleLights.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
          _multipleLights.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
          _multipleLights.SetFloat($"pointLights[{i}].constant", 1.0f);
          _multipleLights.SetFloat($"pointLights[{i}].linear", 0.09f);
          _multipleLights.SetFloat($"pointLights[{i}].quadratic", 0.032f);
        }

        // Spot light
        _multipleLights.SetVector3("spotLight.position", _camera.Position);
        _multipleLights.SetVector3("spotLight.direction", _camera.Front);
        _multipleLights.SetVector3("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
        _multipleLights.SetVector3("spotLight.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
        _multipleLights.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
        _multipleLights.SetFloat("spotLight.constant", 1.0f);
        _multipleLights.SetFloat("spotLight.linear", 0.09f);
        _multipleLights.SetFloat("spotLight.quadratic", 0.032f);
        _multipleLights.SetFloat("spotLight.cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
        _multipleLights.SetFloat("spotLight.outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(17.5f)));

        for (int i = 0; i < _cubePositions.Length; i++)
        {
          Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
          float angle = 20.0f * i;
          model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
          _multipleLights.SetMatrix4("model", model);

          GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        GL.BindVertexArray(_vaoLamp);

        _lampShader.Use();

        _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
        // We use a loop to draw all the lights at the proper position
        for (int i = 0; i < _pointLightPositions.Length; i++)
        {
          Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
          lampMatrix = lampMatrix * Matrix4.CreateTranslation(_pointLightPositions[i]);

          _lampShader.SetMatrix4("model", lampMatrix);

          GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
      }

      if (_currentShader == _shader)
      {
        // Rendering the cube with texture without lighting
        GL.BindVertexArray(_vertexArrayObject);
        _texture.Use(TextureUnit.Texture0);
        _currentShader.Use();

        // Configuring matrices for the shader
        _currentShader.SetMatrix4("model", Matrix4.Identity);
        _currentShader.SetMatrix4("view", _camera.GetViewMatrix());
        _currentShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        // Drawing the cube with 36 vertices for all faces
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }

#if CG_Gizmo
      Gizmo_Sru3D();
#endif
      SwapBuffers();
    }


    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      if (cubo != null)
      {
        ((Cubo)cubo).RotacionarFilho(0.05f);
      }

      #region Teclado
      var estadoTeclado = KeyboardState;
      if (estadoTeclado.IsKeyDown(Keys.Escape))
        Close();
      if (estadoTeclado.IsKeyDown(Keys.D0))
      {
        _currentShader = _shader;
      }
      if (estadoTeclado.IsKeyDown(Keys.D1))
      {
        _currentShader = _basicLighting;
      }
      if (estadoTeclado.IsKeyDown(Keys.D2))
      {
        _currentShader = _lightingMaps;

        {
          _vaoModel = GL.GenVertexArray();
          GL.BindVertexArray(_vaoModel);

          // All of the vertex attributes have been updated to now have a stride of 8 float sizes.
          var positionLocation = _lightingMaps.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

          var normalLocation = _lightingMaps.GetAttribLocation("aNormal");
          GL.EnableVertexAttribArray(normalLocation);
          GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

          // The texture coords have now been added too, remember we only have 2 coordinates as the texture is 2d,
          // so the size parameter should only be 2 for the texture coordinates.
          var texCoordLocation = _lightingMaps.GetAttribLocation("aTexCoords");
          GL.EnableVertexAttribArray(texCoordLocation);
          GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }

        {
          _vaoLamp = GL.GenVertexArray();
          GL.BindVertexArray(_vaoLamp);

          GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

          // The lamp shader should have its stride updated aswell, however we dont actually
          // use the texture coords for the lamp, so we dont need to add any extra attributes.
          var positionLocation = _lampShader.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        }
      }
      if (estadoTeclado.IsKeyDown(Keys.D3))
      {
        _currentShader = _directionalLights;

        {
          _vaoModel = GL.GenVertexArray();
          GL.BindVertexArray(_vaoModel);

          var positionLocation = _directionalLights.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

          var normalLocation = _directionalLights.GetAttribLocation("aNormal");
          GL.EnableVertexAttribArray(normalLocation);
          GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

          var texCoordLocation = _directionalLights.GetAttribLocation("aTexCoords");
          GL.EnableVertexAttribArray(texCoordLocation);
          GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }

        {
          _vaoLamp = GL.GenVertexArray();
          GL.BindVertexArray(_vaoLamp);

          GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

          var positionLocation = _lampShader.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        }
      }
      if (estadoTeclado.IsKeyDown(Keys.D4))
      {
        _currentShader = _pointLights;

        {
          _vaoModel = GL.GenVertexArray();
          GL.BindVertexArray(_vaoModel);

          var positionLocation = _pointLights.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

          var normalLocation = _pointLights.GetAttribLocation("aNormal");
          GL.EnableVertexAttribArray(normalLocation);
          GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

          var texCoordLocation = _pointLights.GetAttribLocation("aTexCoords");
          GL.EnableVertexAttribArray(texCoordLocation);
          GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }

        {
          _vaoLamp = GL.GenVertexArray();
          GL.BindVertexArray(_vaoLamp);

          GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

          var positionLocation = _lampShader.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        }
      }
      if (estadoTeclado.IsKeyDown(Keys.D5))
      {
        _currentShader = _spotlight;

        {
          _vaoModel = GL.GenVertexArray();
          GL.BindVertexArray(_vaoModel);

          var positionLocation = _spotlight.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

          var normalLocation = _spotlight.GetAttribLocation("aNormal");
          GL.EnableVertexAttribArray(normalLocation);
          GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

          var texCoordLocation = _spotlight.GetAttribLocation("aTexCoords");
          GL.EnableVertexAttribArray(texCoordLocation);
          GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }

        {
          _vaoLamp = GL.GenVertexArray();
          GL.BindVertexArray(_vaoLamp);

          GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

          var positionLocation = _lampShader.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        }
      }
      if (estadoTeclado.IsKeyDown(Keys.D6))
      {
        _currentShader = _multipleLights;

        {
          _vaoModel = GL.GenVertexArray();
          GL.BindVertexArray(_vaoModel);

          var positionLocation = _multipleLights.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

          var normalLocation = _multipleLights.GetAttribLocation("aNormal");
          GL.EnableVertexAttribArray(normalLocation);
          GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

          var texCoordLocation = _multipleLights.GetAttribLocation("aTexCoords");
          GL.EnableVertexAttribArray(texCoordLocation);
          GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }

        {
          _vaoLamp = GL.GenVertexArray();
          GL.BindVertexArray(_vaoLamp);

          GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

          var positionLocation = _lampShader.GetAttribLocation("aPos");
          GL.EnableVertexAttribArray(positionLocation);
          GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        }
      }

      const float cameraSpeed = 1.5f;
      var front = Vector3.Normalize(_origin - _camera.Position);
      var right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
      var up = Vector3.Normalize(Vector3.Cross(right, front));

      if (estadoTeclado.IsKeyDown(Keys.R))
        _camera.Position = Vector3.UnitZ * 5;
      if (estadoTeclado.IsKeyDown(Keys.W))
        _camera.Position += front * cameraSpeed * (float)e.Time;
      if (estadoTeclado.IsKeyDown(Keys.S))
        _camera.Position -= front * cameraSpeed * (float)e.Time;
      if (estadoTeclado.IsKeyDown(Keys.A))
        _camera.Position -= right * cameraSpeed * (float)e.Time;
      if (estadoTeclado.IsKeyDown(Keys.D))
        _camera.Position += right * cameraSpeed * (float)e.Time;
      if (estadoTeclado.IsKeyDown(Keys.RightShift))
        _camera.Position += up * cameraSpeed * (float)e.Time;
      if (estadoTeclado.IsKeyDown(Keys.LeftShift))
        _camera.Position -= up * cameraSpeed * (float)e.Time;

      #endregion

      #region  Mouse
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

      GL.DeleteBuffer(_vertexBufferObjectFront_sruEixos);
      GL.DeleteVertexArray(_vertexArrayObjectFront_sruEixos);

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
      GL.BindVertexArray(_vertexArrayObjectFront_sruEixos);
      // Textura
      _shader.SetMatrix4("model", model);
      _shader.SetMatrix4("view", _camera.GetViewMatrix());
      _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shader.Use();
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

  }
}