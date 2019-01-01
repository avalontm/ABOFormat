    ######Como usar ABOFormat
     using ABOFormat; // <== Referencia  
       
       
       ABOAnimation Char; 
        
        protected override void Initialize()
        {
            /* Iniciamos ABOFormat */
            ABOEngine.Init(this); // Iniciamos ABOFormat

            base.Initialize();
        }


        protected override void LoadContent()
        {
            Char = new ABOAnimation(); //Creamos la clase para el personaje

            bool isLoaded = Char.Load(Content.RootDirectory +"/chars/kim.abochar"); //cargamos el archivo .abochar

            if (isLoaded)
            {
                Char.Play("stand"); //Asi estableci el nombre de la animacion en el editor.
                Char.Position = new Vector2(400,280);
                Console.WriteLine("[Loaded] OK");
            }
            else
            {
                Console.WriteLine("[Loaded] ERROR");
            }
        }
        
        
        protected override void Update(GameTime gameTime)
        {
          Char.Update(gameTime);
        }
        
        
        protected override void Draw(GameTime gameTime)
        {
          Char.Draw();
        }
