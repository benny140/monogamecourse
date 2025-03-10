﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogamecourse;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D spriteTarget;
    Texture2D spriteCrosshair;
    Texture2D spriteBackground;
    SpriteFont fontGallery;
    MouseState mouseState;

    Vector2 targetPosition = new Vector2(300, 300);

    private const int targetRadius = 45;
    private Random random = new Random();
    private float timer = 10f; // 10-second timer
    private bool isTimerRunning = true; // Track if the timer is active
    private int score = 0; // Track the player's score

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        fontGallery = Content.Load<SpriteFont>("galleryFont");
        spriteCrosshair = Content.Load<Texture2D>("crosshairs");
        spriteBackground = Content.Load<Texture2D>("sky");
        spriteTarget = Content.Load<Texture2D>("target");
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        if (isTimerRunning)
        {
            // Update the timer
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer <= 0)
            {
                timer = 0;
                isTimerRunning = false;
            }

            // Check if user has scored
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                float distance = Vector2.Distance(
                    new Vector2(mouseState.X - targetRadius, mouseState.Y - targetRadius),
                    targetPosition
                );
                if (distance <= targetRadius)
                {
                    score++;
                    targetPosition = GenerateRandomPosition();
                }
            }
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(spriteBackground, new Vector2(0, 0), Color.White);
        _spriteBatch.DrawString(fontGallery, $"Score: {score}", new Vector2(0, 0), Color.White);
        _spriteBatch.DrawString(
            fontGallery,
            $"Time: {(int)timer}",
            new Vector2(0, 30),
            Color.White
        );
        _spriteBatch.Draw(spriteTarget, targetPosition, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private Vector2 GenerateRandomPosition()
    {
        int screenWidth = GraphicsDevice.Viewport.Width;
        int screenHeight = GraphicsDevice.Viewport.Height;

        int randomX = random.Next(0, screenWidth - targetRadius); // Adjust for target size
        int randomY = random.Next(0, screenHeight - targetRadius); // Adjust for target size

        return new Vector2(randomX, randomY);
    }
}
