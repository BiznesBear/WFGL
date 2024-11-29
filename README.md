# WFGL
"Windows forms game library" allows you to create small 2D games faster (in windows forms, of course). 

> [!NOTE]
> WFGL was created as a 4fun project. This *library* was not created with the intention of creatin large games in it (or even anything in it). 
> Treat this project as a kind of joke/meme. 
> The structure of this project is more like game engine than actually game library.

> [!IMPORTANT]
> WFGL uses GDI+ graphics which may result in low fps if there are too many bitmaps on view. 

## 📄 General futures 
- Auto scaling
- Very basic physics (rect colliders and very simple raycasting)
- Built-in object managment
- Optimalizations
- UI


## 🎈 Install by Cloning
```
$ https://github.com/BiznesBear/WFGL.git
```

> [!IMPORTANT]
> You need minimum .NET 9.0 to use WFGL

When you create a new console project you have to set target platform to Windows and add
```csproj
<UseWindowsForms>true</UseWindowsForms>
```
to your project file.


## ⭐ Showcase
https://github.com/user-attachments/assets/a8b30b77-9fb4-4d3d-88e3-1df1ebba3904

[Code](https://github.com/BiznesBear/FlappyBird)
