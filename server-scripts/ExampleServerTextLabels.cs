using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
using DasNiels.AltV.Streamers;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace TestServer
{
    public class ExampleServerTextLabels : AsyncResource
    {
        public override void OnStart( )
        {
            // YOU MUST ADD THIS IN THE ONSTART OF YOUR GAMEMODE, OBJECTSTREAMER WONT WORK WITHOUT IT!
            AltEntitySync.Init( 1, 100,
               repository => new ServerEventNetworkLayer( repository ),
               ( ) => new LimitedGrid3( 50_000, 50_000, 100, 10_000, 10_000, 600 ),
               new IdProvider( ) 
            );
            //////////////////////////

            AltAsync.OnPlayerConnect += OnPlayerConnect;
            AltAsync.OnConsoleCommand += OnConsoleCommand;

            // Spawn textLabels
            CreateTextLabels( );

            // Display commands in console
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine( "|---------------------AVAILABLE CONSOLE COMMANDS:---------------------|" );
            Console.WriteLine( "| dam -> Destroy all created textLabels." );
            Console.WriteLine( "| cam -> Create all textLabels defined in the CreateTextLabels method." );
            Console.WriteLine( " " );
            Console.WriteLine( "| cp {id} -> Move a specified textLabel by 5 units on the Z axis(height)." );
            Console.WriteLine( "| cc {id} -> Change color of the specified textlabel." );
            Console.WriteLine( "| cs {id} -> Change scale of the specified textLabel." );
            Console.WriteLine( " " );
            Console.WriteLine( "| do {id} -> Destroy a dynamic textLabel by ID(IDs start at 0)." );
            Console.WriteLine( "| go {id} -> Get dynamic textLabel data of the specified textLabel ID." );
            Console.WriteLine( " " );
            Console.WriteLine( "| counttextlabels -> Get the amount of created textLabels." );
            Console.WriteLine( "|--------------------------------------------------------------------|" );
            Console.ResetColor( );
        }

        private void CreateTextLabels( )
        {
            // Create some textLabels
            TextLabelStreamer.CreateDynamicTextLabel( "Some Text", new Vector3( -879.655f, -853.499f, 19.566f ), 0, true, new Rgba( 255, 255, 255, 255 ) );
            TextLabelStreamer.CreateDynamicTextLabel( "Another textlabel", new Vector3( -869.655f, -853.499f, 19.566f ), 0, true, new Rgba( 25, 231, 125, 255 ) );
            TextLabelStreamer.CreateDynamicTextLabel( "[SOME MORE TEXT]", new Vector3( -859.655f, -853.499f, 19.566f ), 0, true, new Rgba( 125, 10, 250, 255 ) );
        }

        private async Task OnConsoleCommand( string name, string[ ] args )
        {
            // destroy all textLabels
            if( name == "dao" )
            {
                TextLabelStreamer.DestroyAllDynamicTextLabels( );
                Console.WriteLine( $"all textLabels destroyed." );
            }

            // create all textLabels
            if( name == "cao" )
            {
                TextLabelStreamer.DestroyAllDynamicTextLabels( );
                CreateTextLabels( );
            }

            // destroy textLabel
            if( name == "do" )
            {
                if( args.Length == 0 )
                    return;

                ulong textLabelId = Convert.ToUInt64( args[ 0 ] );
                if( TextLabelStreamer.DestroyDynamicTextLabel( textLabelId ) )
                {
                    Console.WriteLine( $"TextLabel with ID { textLabelId } deleted!" );
                }
            }

            // change scale
            if( name == "cs" )
            {
                if( args.Length == 0 )
                    return;

                ulong textLabelId = Convert.ToUInt64( args[ 0 ] );
                var textLabel = TextLabelStreamer.GetDynamicTextLabel( textLabelId );
                if( textLabel != null )
                {
                    textLabel.Scale += 1;
                    Console.WriteLine( $"TextLabel scale increased by 1." );
                }
                else
                    Console.WriteLine( $"Couldnt find textLabel with ID { textLabelId }" );
            }

            // change color
            if( name == "cc" )
            {
                if( args.Length == 0 )
                    return;

                ulong textLabelId = Convert.ToUInt64( args[ 0 ] );
                var textLabel = TextLabelStreamer.GetDynamicTextLabel( textLabelId );
                if( textLabel != null )
                {
                    Random r = new Random( );
                    textLabel.Color = new Rgba( ( byte ) r.Next( 0, 256 ), ( byte ) r.Next( 0, 256 ), ( byte ) r.Next( 0, 256 ), 255 );
                    Console.WriteLine( $"Textlabel color changed to random value" );
                }
                else
                    Console.WriteLine( $"Couldnt find textLabelect with ID { textLabelId }" );
            }

            // change pos
            if( name == "cp" )
            {
                if( args.Length == 0 )
                    return;

                ulong textLabelId = Convert.ToUInt64( args[ 0 ] );
                var textLabel = TextLabelStreamer.GetDynamicTextLabel( textLabelId );
                if( textLabel != null )
                {
                    Console.WriteLine( $"textLabel pos: { textLabel.Position.Z }" );

                    textLabel.Position += new Vector3( 0, 0, 5 );
                    Console.WriteLine( $"TextLabel position increased on Z with +5f { textLabel.Position.Z }" );
                }
                else
                    Console.WriteLine( $"Couldnt find textLabel with ID { textLabelId }" );
            }

            // get textLabel by ID
            if( name == "go" )
            {
                if( args.Length == 0 )
                    return;

                ulong textLabelId = Convert.ToUInt64( args[ 0 ] );
                var textLabel = TextLabelStreamer.GetDynamicTextLabel( textLabelId );
                if( textLabel != null )
                {
                    Console.WriteLine( $"TextLabel found, data: { textLabel.Text }, { textLabel.Scale }, { textLabel.Center }, { textLabel.Font }, ...!" );
                }
                else
                    Console.WriteLine( $"Couldnt find textLabel with ID { textLabelId }" );
            }

            // count textLabels
            if( name == "counttextlabels" )
            {
                Console.WriteLine( $"total textLabels created: { TextLabelStreamer.GetAllDynamicTextLabels( ).Count }" );
            }
        }

        private async Task OnPlayerConnect( IPlayer player, string reason )
        {
            Console.WriteLine( $"{ player.Name } connected!" );
            player.Model = ( uint ) AltV.Net.Enums.PedModel.FreemodeMale01;
            player.Spawn( new Position( -889.655f, -853.499f, 20.566f ), 0 );
        }

        public override void OnStop( )
        {
            TextLabelStreamer.DestroyAllDynamicTextLabels( );
            Console.WriteLine( $"Server stopped." );
        }
    }
}
