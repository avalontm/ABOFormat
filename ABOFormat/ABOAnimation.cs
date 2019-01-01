using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ABOFormat
{
    public class ABOAnimation
    {
        int fps = 30;

        List<CharProperties> Animations;
        CharProperties Animation;
        SpriteProperties sprite;

        SpriteEffects effectX;
        SpriteEffects effectY;
        SpriteEffects FlipX;

        int IndexAnimation;
        double gameTime;
        int time;

        public string CurrentAnimation { private set; get; }
        public int FrameCurrent { private set; get; }
        public Vector2 Position;
        public CharProperties BackAnimation { private set; get; }
        public bool IsLoaded { private set; get; }

        public bool FlipHorizontal = false;

        public ABOAnimation()
        {
            Animations = new List<CharProperties>();
            Position = new Vector2();
            FrameCurrent = 0;
            CurrentAnimation = "";
            IndexAnimation = -1;
            IsLoaded = false;
            time = 0;
        }

        public int Frames
        {
            get
            {
                try
                {
                    return (Animation.Textures.Count - 1);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public bool Load(string file)
        {
            if (File.Exists(file))
            {
                var readBytes = File.ReadAllBytes(file);
                IsLoaded = Desserialize(readBytes);
                return IsLoaded;
            }

            return false;
        }

        void BackToAnimation(string AniName)
        {
            int index = -1;

            try
            {
                index = Animations.IndexOf(Animations.Single(a => a.Name.ToLower() == AniName.ToLower()));
            }
            catch
            {
                index = -1;
            }

            if (index >= 0 && index < Animations.Count)
            {
                BackAnimation = Animations[index];
            }
            else
            {
                BackAnimation = null;
            }
        }

        public bool Play(string AniName, string BackToAnim = "")
        {
            int index = -1;

            if (IsLoaded)
            {
                if (CurrentAnimation.ToLower() != AniName.ToLower())
                {
                    try
                    {
                        index = Animations.IndexOf(Animations.Single(a => a.Name.ToLower() == AniName.ToLower()));
                    }
                    catch
                    {
                        index = -1;
                    }

                    BackAnimation = null;
                    if (!string.IsNullOrEmpty(BackToAnim))
                    {
                        BackToAnimation(BackToAnim);
                    }
                    return onPlayName(index, AniName);
                }
            }
                return false;
        }

        bool onPlayName(int index, string AniName)
        {
            if (IsLoaded)
            {
                if (index >= 0 && index < Animations.Count)
                {
                    Animation = Animations[index];

                    if (Animation != null)
                    {
                        CurrentAnimation = Animation.Name;
                        IndexAnimation = index;
                        FrameCurrent = 0;
                        time = 0;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Play(int Index, string BackToAnim = "")
        {
            if (IsLoaded)
            {
                if (IndexAnimation != Index)
                {
                    BackAnimation = null;
                    if (!string.IsNullOrEmpty(BackToAnim))
                    {
                        BackToAnimation(BackToAnim);
                    }
                    return onPlayIndex(Index);
                }
            }
            return false;
        }

        bool onPlayIndex(int Index)
        {
            if (IsLoaded)
            {
                if (Index >= 0 && Index < Animations.Count)
                {
                    Animation = null;
                    Animation = Animations[Index];

                    if (Animation != null)
                    {
                        CurrentAnimation = Animation.Name;
                        IndexAnimation = Index;
                        FrameCurrent = 0;
                        time = 0;
                        return true;
                    }
                }
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime.TotalGameTime.Milliseconds;
            if (IsLoaded)
            {
                Animator();
            }
        }

         
        float FixFlipX
        {
            get
            {
                if (FlipHorizontal)
                {
                    if (Animation != null)
                    {
                        FlipX = SpriteEffects.FlipHorizontally;
                        return (Animation.Textures[FrameCurrent].Position.X*2) + (Animation.Textures[FrameCurrent].Texture.Width);
                    }
                }
                FlipX = SpriteEffects.None;
                return 0;
            }
        }

        public void Draw()
        {
            if (IsLoaded)
            {
                if (Animation != null)
                {
                    if (Animation.Textures != null)
                    {
                        effectX = SpriteEffects.None;
                        effectY = SpriteEffects.None;

                        if (Animation.Textures[FrameCurrent].TurnX)
                        {
                            effectX = SpriteEffects.FlipHorizontally;
                        }

                        if (Animation.Textures[FrameCurrent].TurnY)
                        {
                            effectY = SpriteEffects.FlipVertically;
                        }

                        ABOEngine.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, null);
                        ABOEngine.SpriteBatch.Draw(Animation.Textures[FrameCurrent].Texture, new Vector2(Position.X + Animation.Textures[FrameCurrent].Position.X - FixFlipX, Position.Y + Animation.Textures[FrameCurrent].Position.Y), null, Color.White, 0, new Vector2(0, 0), 1, (effectX| FlipX) | effectY, 0);
                        ABOEngine.SpriteBatch.End();
                    }
                }
            }
        }

        void Animator()
        {
            if (Animation != null)
            {
                if (Animation.Textures != null)
                {
                    if (FrameCurrent > Animation.Textures.Count)
                    {
                        FrameCurrent = 0;
                    }

                    sprite = Animation.Textures[FrameCurrent];

                    if (time > sprite.Wait)
                    {
                        time = 0;
                        FrameCurrent = Math.Min(FrameCurrent + 1, Animation.Textures.Count);
                    }

                    if (FrameCurrent == Animation.Textures.Count)
                    {
                        if (Animation.Loop)
                        {
                            FrameCurrent = 0;
                        }
                        else
                        {
                            FrameCurrent = Animation.Textures.Count-1;

                            if (BackAnimation != null)
                            {
                                time = 0;
                                Animation = BackAnimation;
                            }
                        }
                    }

                    time += fps;
                }
            } 
        }

        /* =================================================================================================================================================================== */
        /* LEER ARCHIVO */
        /* =================================================================================================================================================================== */

        bool Desserialize(byte[] data)
        {
            try
            {
                Animations.Clear();

                using (MemoryStream m = new MemoryStream(data))
                {
                    using (BinaryReader reader = new BinaryReader(m))
                    {
                        string _header = "ABOCHAR";
                        string Header = Encoding.ASCII.GetString(reader.ReadBytes(_header.Length)); //Obtenemos el Header

                        int Ani_Total = reader.ReadInt32();

                        if (Header == _header)
                        {
                            for (int a = 0; a < (Ani_Total); a++)
                            {
                                CharProperties animation = new CharProperties();

                                int name_length = reader.ReadInt32();
                                animation.Name = Encoding.ASCII.GetString(reader.ReadBytes(name_length)); // Obtenemos el nombre de la animacion

                                bool loop = Convert.ToBoolean(reader.ReadInt32());
                                animation.Loop = loop;

                                int Text_Total = reader.ReadInt32();

                                animation.Textures = new List<SpriteProperties>();

                                for (int t = 0; t < (Text_Total); t++)
                                {
                                    SpriteProperties Sprite = new SpriteProperties();

                                    int wait = reader.ReadInt32();
                                    bool turnX = Convert.ToBoolean(reader.ReadInt32());
                                    bool turnY = Convert.ToBoolean(reader.ReadInt32());

                                    Sprite.Wait = wait;
                                    Sprite.TurnX = turnX;
                                    Sprite.TurnY = turnY;

                                    int x = reader.ReadInt32();
                                    int y = reader.ReadInt32();

                                    Sprite.Position = new Vector2(x, y);

                                    int Sprite_Size = reader.ReadInt32();

                                    Sprite.Texture = toTexture(reader.ReadBytes(Sprite_Size));

                                    animation.Textures.Add(Sprite);
                                }

                                Animations.Add(animation);
                            }

                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                Animations.Clear();
                return false;
            }
        }

        Texture2D toTexture(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Texture2D returnImage = Texture2D.FromStream(ABOEngine.Game.GraphicsDevice, ms);
                return returnImage;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error [toTexture]: " + ex);
                return new Texture2D(ABOEngine.Game.GraphicsDevice, 1, 1);
            }
        }
    }
}
