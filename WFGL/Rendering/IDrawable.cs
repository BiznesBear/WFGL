using WFGL.Core;
using WFGL.Objects;
namespace WFGL.Rendering;
public interface IDrawable
{
    public Hroup? Hroup { get; set; }
    public void Draw(GameMaster m, Graphics r);
}
