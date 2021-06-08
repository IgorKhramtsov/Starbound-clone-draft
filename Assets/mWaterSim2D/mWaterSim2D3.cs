// 2D Water Sim Converted to Unity C# - mgear - http://unitycoder.com/blog
// controls: only left/right button added so far..
// ORIGINAL SOURCE: http://w-shadow.com/blog/2009/09/01/simple-fluid-simulation/

using UnityEngine;
using System.Collections;

public class mWaterSim2D3 : MonoBehaviour 
{

		//Block types
	public const int AIR = 0;
	public const  int GROUND = 1;
	public const  int WATER = 2;

	//Water properties
	public const  float MaxMass = 1.0f;
	public const  float MaxCompress = 0.02f;
	public const  float MinMass = 0.0001f;

	public const  float MinDraw = 0.01f;
	public const  float MaxDraw = 1.1f;

	public const  float MaxSpeed = 1f;   //max units of water moved out of one block to another, per timestep

	public const  float MinFlow = 0.01f;

	//Define map dimensions and data structures
	public const  int map_width = 32;   
	public const  int map_height =32;

//	int[][] blocks = new int[map_width+2][];
	private int[,] blocks = new int[map_width+2, map_height+2];
	//[map_width+2][map_height+2];

	private float[,] mass = new float[map_width+2,map_height+2];
	private float[,] new_mass = new float[map_width+2,map_height+2];

	//Window size
	public const  int display_width = 128;
	public const  int display_height = 128;
	//Set the size of the top panel
	public const  int panel_height = 0;
	int	panel_width = display_width;

	//Block size will be automatically calculated based on the above settings.
	int block_width, block_height, hblock_width, hblock_height;
			  
	//Define colors
	private Color[] block_colors = {
	new Color(0,0,0,1),        //air
	  new Color(0.5f,0.5f,0.5f,1) //ground
	};

	//Mouse highlight
	private Color highlight = new Color(0, 1, 0, 1);

	//..and the font
	//PFont font;

	//Control variables
	public bool stepping = false;   //run the simulation in stepping mode (press Space to run a single step)
	public bool grid = false;       //show the grid
	public bool draw_depth = true; //draw partially filled cells differently, simulating more
								//fine-grained water depth display (imperfect)
								
	public const  int SHOW_NOTHING = 0;                            
	public const  int SHOW_MASS = 1;
	int show_state = SHOW_NOTHING; //whether to show the mass of each block

	public Texture2D texture;
	
	// Use this for initialization
	void Start () 
	{
		

	  block_width = display_width / map_width;
	  block_height = display_height / map_height;
		
	  hblock_width = block_width / 2;
	  hblock_height = block_height / 2;

	// texture
	texture = new Texture2D(map_width*block_width, map_height*block_height);
//	texture = new Texture2D(map_width, map_height);
	GetComponent<Renderer>().material.mainTexture = texture;

		
//	  panel_width = block_width * map_width;
	  
	  //create a random map
	  initmap();
	  
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		// mouse control
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100))
			{
				int block = -1;
				block = WATER;
				
				var pixelUV = hit.textureCoord;
				pixelUV.x *= texture.width;
				pixelUV.y *= texture.height;
				int mx = (int) (pixelUV.x/block_width);
				int my = (int) (pixelUV.y/block_height);
				
			  blocks[mx,my] = block;
			  mass[mx,my] = new_mass[mx,my] = block == WATER ? MaxMass : 0;
			}
		}

		if (Input.GetMouseButton(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100))
			{
				int block = -1;
				block = GROUND;
				
				var pixelUV = hit.textureCoord;
				pixelUV.x *= texture.width;
				pixelUV.y *= texture.height;
				int mx = (int) (pixelUV.x/block_width);
				int my = (int) (pixelUV.y/block_height);
				
			  blocks[mx,my] = block;
			  mass[mx,my] = new_mass[mx,my] = block == WATER ? MaxMass : 0;
			}
		}

		
	// clear texture	
	  for ( int x = 0; x < map_height; x++ ){
		for ( int y = 0; y < map_height; y++ ){
			texture.SetPixel(x, y, new Color(0,0,0,1));
		}
	}
	texture.Apply();
	
//		  int mx = constrain( int(mouseX / block_width)+1, 1, map_width );
//		  int my = constrain( map_height - int( (mouseY - panel_height) / block_height), 1, map_height );
//		  boolean shift = keyPressed && (key == CODED) && (keyCode == SHIFT);
		  
/*		  //Edit the map if the mouse is pressed 
		  if ( mousePressed ){
			int block = -1;
			
			if (mouseButton == LEFT){
			  block = GROUND;
			} else if (mouseButton == RIGHT){
			  block = WATER;
			}
			
			if (shift) {
			  if ( blocks[mx][my] == block ){
				block = AIR;
			  } else {
				block = -1;
			  }
			}
			
			if ( block != -1 ){
			  blocks[mx][my] = block;
			  mass[mx][my] = new_mass[mx][my] = block == WATER ? MaxMass : 0;
			}
		  }
*/		  
		  //Run the water simulation (unless we're in the step-by-step mode)
		  if (!stepping){
			simulate();
		  }
		  
		  //Draw the map
		  float h = 0;
		  Color c;
		  
//		  textSize( 12 );
//		  textAlign( CENTER, CENTER );
		  
//		  if ( grid ){
//			stroke(0);
//		  } else {
//			noStroke();
//		  }
		  
		  for ( int x = 1; x <= map_height; x++ ){
			for ( int y = 1; y <= map_height; y++ ){
			  if ( blocks[x,y] == WATER ){
				
				//Skip cells that contain very little water
				if (mass[x,y] < MinDraw) continue;
				
				//Draw water
				if ( draw_depth && ( mass[x,y] < MaxMass ) ){
				  //Draw a half-full block. Block size is dependent on the amount of water in it.
				  if ( mass[x,y+1] >= MinDraw ){
					draw_block(x, y, waterColor(mass[x,y+1]), 1);
				  }
				  draw_block( x, y, waterColor(mass[x,y]), mass[x,y]);
				} else {
				  //Draw a full block
				  h = 1;
				  c = waterColor( mass[x,y] );
				  draw_block( x, y, c, h);
				}
				
			  } else {
				//Draw any other block
				draw_block( x, y, block_colors[ blocks[x,y] ], 1 );
			  }
			  
			}  
		  }
		  
		  texture.Apply();
		  
//		  draw_marching(); //Marching squares. Doesn't look very good.
		  
		  //Draw the highlight under the mouse
//		  noStroke();
//		  draw_block( mx , my, highlight, 1);
		  
//		  draw_panel();

		
	}
	
	
	//Fill the map with random blocks
	void initmap()
	{
	  for ( int x = 0; x < map_height + 2; x++ ){
		for ( int y = 0; y < map_height + 2; y++ ){
		  blocks[x,y] = (int) Random.Range(0, 3);
		  mass[x,y] = blocks[x,y] == WATER ? MaxMass : 0.0f;
		  new_mass[x,y] = blocks[x,y] == WATER ? MaxMass : 0.0f;
		}  
	  } 
	  
	 for (int x =0; x < map_width+2; x++){
		blocks[x,0] = AIR;
		blocks[x,map_height+1] = AIR;
	  }
	  
	  for (int y = 1; y < map_height+1; y++){
		blocks[0,y] = AIR;
		blocks[map_width+1,y] = AIR;
	  }

	}
	
	
	void draw_block( int x, int y, Color c, float filled )
	{
	  int screen_xx = (int) screen_x( x - 1 );
	  int screen_yy = (int) screen_y( y - 1 );
	  //float block_yy = screen_y(y - 1 + filled);
	  int block_yy = (int) screen_y(y - 1 + filled);
	  
		
	// draw block
	for (int py =0; py <  block_height; py++)
		{
		for (int px = 0; px < block_width; px++)
			{
				texture.SetPixel((int) x*block_width+px, (int) y*block_height+py, c);
			}
		}
//				texture.SetPixel(x,y, c);
//	  for (int py = block_yy; py <  screen_yy+block_yy; py++)
//		{
//		for (int px = screen_xx; px < screen_xx+block_width; px++)
//			{
//				texture.SetPixel((int) px, (int) py, c);
//			}
//		}
		
		
		
//	  fill(c);
//	  rect( screen_x, block_y, block_width, screen_y - block_y );
	  
	  if ( (show_state == SHOW_MASS) && ( mass[x,y] >= MinMass )){
//		fill( 0, 0, 0, 255);
//		text( mass[x,y] , screen_x + hblock_width, screen_y - hblock_height);
	  }
	}
	
	
	//Calculates an RGB water color based on the amount of water in the cell
	Color waterColor(float m)
	{
	  m = Mathf.Clamp( m, MinDraw, MaxDraw );
	  
	  int r = 50, g = 50;
	  int b;
	  
	  if (m < 1){
		b = (int) map(m, 0.01f, 1, 200, 255);
		r = (int) map(m, 0.01f, 1, 50, 240);
		r = Mathf.Clamp(r, 50, 240);
		g = r;
	  } else {
		b = (int) map(m, 1.0f, 1.1f, 140, 190);
	  }
	  
	  b = Mathf.Clamp(b, 140, 255);
	  
//	  return new Color(r/256,g/256,b/256,1);
	  return new Color(0,0,m,1);
//	  return new Color(r/200,g/200,b/200,1);
	}
	
	void simulate(){
		simulate_compression();
	}

	// custom function
	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
	
	
	void simulate_compression()
	{
		  float Flow = 0;
		  float remaining_mass;
		  
		  //Calculate and apply flow for each block
		  for (int x = 1; x <= map_width; x++){
			 for(int y = 1; y <= map_height; y++){
			   //Skip inert ground blocks
			   if ( blocks[x,y] == GROUND) continue;
			   
			   //Custom push-only flow
			   Flow = 0;
			   remaining_mass = mass[x,y];
			   if ( remaining_mass <= 0 ) continue;
			   
			   //The block below this one
			   if ( (blocks[x,y-1] != GROUND) ){
				 Flow = get_stable_state_b( remaining_mass + mass[x,y-1] ) - mass[x,y-1];
				 if ( Flow > MinFlow ){
				   Flow *= 0.5f; //leads to smoother flow
				 }
				 Flow = Mathf.Clamp(Flow, 0, Mathf.Min(MaxSpeed, remaining_mass));
				 
				 new_mass[x,y] -= Flow;
				 new_mass[x,y-1] += Flow;   
				 remaining_mass -= Flow;
			   }
			   
			   if ( remaining_mass <= 0 ) continue;
			   
			   //Left
			   if ( blocks[x-1,y] != GROUND ){
				 //Equalize the amount of water in this block and it's neighbour
				 Flow = (mass[x,y] - mass[x-1,y])/4;
				 if ( Flow > MinFlow ){ Flow *= 0.5f; }
				 Flow = Mathf.Clamp(Flow, 0, remaining_mass);
				  
				 new_mass[x,y] -= Flow;
				 new_mass[x-1,y] += Flow;
				 remaining_mass -= Flow;
			   }
			   
			   if ( remaining_mass <= 0 ) continue;

			   //Right
			   if ( blocks[x+1,y] != GROUND ){
				 //Equalize the amount of water in this block and it's neighbour
				 Flow = (mass[x,y] - mass[x+1,y])/4;
				 if ( Flow > MinFlow ){ Flow *= 0.5f; }
				 Flow = Mathf.Clamp(Flow, 0, remaining_mass);
				  
				 new_mass[x,y] -= Flow;
				 new_mass[x+1,y] += Flow;
				 remaining_mass -= Flow;
			   }
			   
			   if ( remaining_mass <= 0 ) continue;
			   
			   //Up. Only compressed water flows upwards.
			   if ( blocks[x,y+1] != GROUND ){
				 Flow = remaining_mass - get_stable_state_b( remaining_mass + mass[x,y+1] );
				 if ( Flow > MinFlow ){ Flow *= 0.5f; }
				 Flow = Mathf.Clamp( Flow, 0, Mathf.Min(MaxSpeed, remaining_mass) );
				 
				 new_mass[x,y] -= Flow;
				 new_mass[x,y+1] += Flow;   
				 remaining_mass -= Flow;
			   }

			   
			 }
		  }
		  
		  //Copy the new mass values to the mass array
		  for (int x = 0; x < map_width + 2; x++){
			for (int y = 0; y < map_height + 2; y++){
			  mass[x,y] = new_mass[x,y];
			}
		  }
		  
		  for (int x = 1; x <= map_width; x++){
			 for(int y = 1; y <= map_height; y++){
			   //Skip ground blocks
			   if(blocks[x,y] == GROUND) continue;
			   //Flag/unflag water blocks
			   if (mass[x,y] > MinMass){
				 blocks[x,y] = WATER;
			   } else {
				 blocks[x,y] = AIR;
			   }
			 }
		  }
		  
		  //Remove any water that has left the map
		  for (int x =0; x < map_width+2; x++){
			mass[x,0] = 0;
			mass[x,map_height+1] = 0;
		  }
		  for (int y = 1; y < map_height+1; y++){
			mass[0,y] = 0;
			mass[map_width+1,y] = 0;
		  }

		}

	
	float get_stable_state_b ( float total_mass ){
		  if ( total_mass <= 1 ){
			return 1;
		  } else if ( total_mass < 2*MaxMass + MaxCompress ){
			return (MaxMass*MaxMass + total_mass*MaxCompress)/(MaxMass + MaxCompress);
		  } else {
			return (total_mass + MaxCompress)/2;
		  }
		}
	
		float screen_x( float x ){
			return x  * block_width;
		}

		float screen_y( float y ){
		  return (map_height - y)*block_height + panel_height;
		}

		
		
} // end class


/*
 * A simple water simulation based on cellular automata
 *
 * by Janis Elsts / W-Shadow
 *
 * Info : http://w-shadow.com/blog/2009/09/01/simple-fluid-simulation/
 */

/*



void draw_panel(){
  fill(120, 250, 100);
  rect(0, 0, panel_width, panel_height);
  int offset = width - panel_width;
  
  fill(0);
  textSize( 16 );
  //Display FPS
  textAlign( RIGHT, CENTER );
  text( round(frameRate) + " FPS", panel_width - 5 - offset, panel_height/2 )  ;

  if ( stepping ) {
    textAlign( LEFT, CENTER );
    text( "Press [Space] to step the simulation", 5, panel_height/2 )  ;
  }
  
  if ( show_state == SHOW_MASS ){
    textAlign( CENTER, CENTER );
    text( "[Mass]", panel_width - 100 - offset, panel_height/2 )  ;
  } 
  
}

//A basic marching squares implementation. For testing only, doesn't animate very well.
void draw_marching(){
  textAlign(CENTER, CENTER);
  fill(0);
  stroke(50);
  textSize(14);
  float screen_x;
  float screen_y;
  
  for ( int x = 1; x <= map_height; x++ ){
    for ( int y = 1; y <= map_height; y++ ){
      //march!        
      
      float mass_f = 0.5;
      float 
        mass_bl = mass[x][y]*mass_f,          //bottom-left
        mass_br = mass[x+1][y]*mass_f,      //bottom-right
        mass_tl = mass[x][y+1]*mass_f,      //top-left
        mass_tr = mass[x+1][y+1]*mass_f;  //top-right
      
      int state;
      
      state = (
        ( (mass_bl >= MinDraw) ? 1 : 0) + 
        ( (mass_br >= MinDraw) ? 2 : 0) +
        ( (mass_tr >= MinDraw) ? 4 : 0) +
        ( (mass_tl >= MinDraw) ? 8 : 0)
      );
      
      float zx = x - 1 + 0.5;
      float zy = y - 1 ;
      
      float px1, py1, px2, py2;
      
      switch (state) {
        case 1 : 
         line(
           screen_x( zx ),
           screen_y( zy + mass_bl),
           screen_x( zx + mass_bl),
           screen_y( zy  ) 
         );
         break;
         
       case 2 : 
         line(
           screen_x( zx + 1 - mass_br),
           screen_y( zy ),
           screen_x( zx + 1 ),
           screen_y( zy + mass_br) 
         );
         break;
         
       case 3 : 
         line(
           screen_x( zx ),
           screen_y( zy + mass_bl),
           screen_x( zx + 1 ),
           screen_y( zy + mass_br ) 
         );
         break;
       
       case 4 : 
         line(
           screen_x( zx + 1 - mass_tr),
           screen_y( zy + 1),
           screen_x( zx + 1 ),
           screen_y( zy + 1 - mass_tr ) 
         );
         break;
         
       case 5 : 
         line(
           screen_x( zx ),
           screen_y( zy + mass_bl),
           screen_x( zx + mass_bl ),
           screen_y( zy ) 
         );
         
         line(
           screen_x( zx + 1 - mass_tr),
           screen_y( zy + 1),
           screen_x( zx + 1 ),
           screen_y( zy + 1 - mass_tr) 
         );         
         break;
         
      case 6 : 
         line(
           screen_x( zx + 1 - mass_tr),
           screen_y( zy + 1),
           screen_x( zx + 1 - mass_br ),
           screen_y( zy ) 
         );
         break; 
         
       case 7 : 
         line(
           screen_x( zx ),
           screen_y( zy + mass_bl ),
           screen_x( zx + 1 - mass_tr ),
           screen_y( zy + 1) 
         );
         break;
         
       case 8 : 
         line(
           screen_x( zx ),
           screen_y( zy + 1 - mass_tl ),
           screen_x( zx + mass_tl ),
           screen_y( zy + 1 ) 
         );
         break;
         
       case 9 : 
         line(
           screen_x( zx + mass_tl),
           screen_y( zy + 1),
           screen_x( zx + mass_bl ),
           screen_y( zy ) 
         );
         break;   
         
       case 10 : 
         line(
           screen_x( zx + mass_tl ),
           screen_y( zy + 1 ),
           screen_x( zx ),
           screen_y( zy + 1 - mass_tl) 
         );
       
        line(
          screen_x( zx + 1 - mass_br),
          screen_y( zy ),
          screen_x( zx + 1 ),
          screen_y( zy + mass_br) 
        );         
        break;
        
       case 11 : 
         line(
           screen_x( zx + mass_tl),
           screen_y( zy + 1 ),
           screen_x( zx + 1 ),
           screen_y( zy + mass_br ) 
         );
         break;
         
       case 12 : 
         line(
           screen_x( zx ),
           screen_y( zy + 1 - mass_tl),
           screen_x( zx + 1 ),
           screen_y( zy + 1 - mass_tr ) 
         );
         break;
         
       case 13 : 
         line(
           screen_x( zx + mass_bl ),
           screen_y( zy ),
           screen_x( zx + 1 ),
           screen_y( zy + 1 - mass_tr ) 
         );
         break;
      
       case 14 : 
         line(
           screen_x( zx ),
           screen_y( zy + 1 - mass_tl),
           screen_x( zx + 1 - mass_br),
           screen_y( zy) 
         );
         break;
         
      }
      
      screen_x = (x - 1)*block_width;
      screen_y = (map_height - y)*block_height + panel_height;
      //text( x + "/" + y, screen_x + block_width, screen_y );
      text( state , screen_x + hblock_width, screen_y + hblock_height);
    }  
  }  
}




//Clear the map
void clearmap(){
  for ( int x = 1; x <= map_height; x++ ){
    for ( int y = 1; y <= map_height; y++ ){
      blocks[x][y] = AIR;
      mass[x][y] = 0;
      new_mass[x][y] = 0;
    }  
  }
  
  for (int x =0; x < map_width+2; x++){
    blocks[x][0] = AIR;
    blocks[x][map_height+1] = AIR;
  }
  
  for (int y = 1; y < map_height+1; y++){
    blocks[0][y] = AIR;
    blocks[map_width+1][y] = AIR;
  }
}

void keyPressed(){
  if (key == CODED) return;
  
  switch(key){
    case 's':  //Toggle step-by-step simulation
    case 'S': 
      stepping = !stepping;
      break;
    
    case 'c':  //Clear the map
    case 'C':
      clearmap();
      break;
      
    case 'r':  //Reinitialize the map with random cells
    case 'R':
      initmap();
      break;
      
    case 'n':  //Toggle mass display
    case 'N':
      show_state++;
      if ( show_state > 1 ) show_state = SHOW_NOTHING;
      break;
      
    case 'g': //Toggle grid
    case 'G':
      grid = !grid;
      if ( grid ){
        stroke(0);
      } else {
        noStroke();
      }
      break;
      
    case 'd':  //Toggle between basic water rendering and depth-based (=mass-based) rendering
    case 'D':
      draw_depth = !draw_depth;
      break;
      
    default: 
      if (stepping) {
        simulate();
      };
  }

}
*/