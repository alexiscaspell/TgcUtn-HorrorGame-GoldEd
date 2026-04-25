// Compilation stubs for Docker build check (shader-heavy lighting classes excluded).
// NOT compiled in real build.
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA
{
    abstract partial class ALuz
    {
        public Mapa mapa { get; set; }
        public CamaraFPS camaraFPS { get; set; }
        virtual public void render() { }
        virtual public void init() { }
        abstract public void configInicial();
        abstract public void configurarEfecto(TgcMesh mesh);
        abstract public void configurarSkeletal(TgcSkeletalMesh mesh);
    }
}

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.faroles
{
    partial class LuzFarol : AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA.ALuz
    {
        public LuzFarol(Mapa mapa, CamaraFPS camaraFPS) { }
        public override void configInicial() { }
        public override void configurarEfecto(TgcMesh mesh) { }
        public override void configurarSkeletal(TgcSkeletalMesh mesh) { }
    }
}

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.fluors
{
    partial class LuzFluor : AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA.ALuz
    {
        public LuzFluor(Mapa mapa, CamaraFPS camaraFPS) { }
        public override void configInicial() { }
        public override void configurarEfecto(TgcMesh mesh) { }
        public override void configurarSkeletal(TgcSkeletalMesh mesh) { }
    }
}

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.linternas
{
    partial class LuzLinterna : AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA.ALuz
    {
        public LuzLinterna(Mapa mapa, CamaraFPS camaraFPS) { }
        public override void configInicial() { }
        public override void configurarEfecto(TgcMesh mesh) { }
        public override void configurarSkeletal(TgcSkeletalMesh mesh) { }
    }
}

namespace AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.general
{
    partial class LuzOscura : AlumnoEjemplos.LOS_IMPROVISADOS.Iluminadores.IyCA.ALuz
    {
        public LuzOscura(Mapa mapa, CamaraFPS camaraFPS) { }
        public override void configInicial() { }
        public override void configurarEfecto(TgcMesh mesh) { }
        public override void configurarSkeletal(TgcSkeletalMesh mesh) { }
    }
}

namespace AlumnoEjemplos.LOS_IMPROVISADOS.EfectosPosProcesado
{
    partial class EfectoEscondido
    {
        public EfectoEscondido(Mapa mapa, CamaraFPS camaraFPS) { }
        public bool terminoEfecto { get; private set; }
        public void render() { }
        public void init() { }
    }
}

namespace AlumnoEjemplos.LOS_IMPROVISADOS
{
    partial class RoomFarol : AlumnoEjemplos.LOS_IMPROVISADOS.ARoomLuz
    {
        public RoomFarol() { }
        public RoomFarol(SharpDX.Vector3 position) { }
        public override void render() { }
    }

    partial class RoomLinterna : AlumnoEjemplos.LOS_IMPROVISADOS.ARoomLuz
    {
        public RoomLinterna() { }
        public override void render() { }
    }
}

namespace AlumnoEjemplos.LOS_IMPROVISADOS.EfectosPosProcesado
{
    partial class PosProcesadoBur : APosProcesado
    {
        public PosProcesadoBur(Mapa mapa) : base(mapa) { }
        public void initRedAndBlur() { }
        public override void init() { }
        public override void render(float elapsedTime) { }
        public override void drawSceneToRenderTarget(SharpDX.Direct3D9.Device d3dDevice) { }
        public override void drawPostProcess(SharpDX.Direct3D9.Device d3dDevice) { }
        public override void close() { }
    }
}
