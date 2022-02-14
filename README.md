# DEPRECATED! USE THIS: https://github.com/CoffeeGen/altv-entity-streamers-os

# ALT:V MP Server-side TextLabel Streamer
A server-side C# implementation of a textlabel streamer for ALT:V MP.

## Installation
- This resource makes use of the ``AltV.Net.EntitySync (v1.0.20-dev-preview)`` and ``AltV.Net.EntitySync.ServerEvent (v1.0.19-dev-preview)`` nuget package, make sure to install those prior to using this resource.
- Copy ``server-scripts/TextLabelStreamer.cs`` to your gamemode.
- Make sure to add the following code to your gamemode's OnStart() method(the textlabel streamer won't work without it!):
```csharp
// Documentation: https://fabianterhorst.github.io/coreclr-module/articles/entity-sync.html
AltEntitySync.Init( 1, 100,
   repository => new ServerEventNetworkLayer( repository ),
   ( ) => new LimitedGrid3( 50_000, 50_000, 100, 10_000, 10_000, 600 ),
   new IdProvider( )
);
```
- Copy ``textlabel-streamer-client`` to your ``server-root/resources`` directory.
- Add ``"textlabel-streamer-client"`` to your server config resources list.

## Usage
The following global methods are available:
```csharp
// Create a new textlabel on the map, returns the created textlabel.
DynamicTextLabel CreateDynamicTextLabel( 
    string text, Vector3 position, int dimension = 0, bool? center = true, Rgba? color = null, int? scale = null,
    DropShadow dropShadow = null, Rgba? edge = null, int? font = null, bool? proportional = null, uint streamRange = 30
);

// Destroy a textlabel by it's ID or textlabel instance. returns true if successful.
bool DestroyDynamicTextLabel( ulong dynamicTextLabelId );
void DestroyDynamicTextLabel( DynamicTextLabel dynamicTextLabel );

// Get a textlabel by it's ID. returns the textlabel if successful or null if not.
DynamicTextLabel GetDynamicTextLabel( ulong dynamicTextLabelId );

// Destroy all created textlabels.
void DestroyAllDynamicTextLabels( );

// Get a list of all created textlabels.
List<DynamicTextLabel> GetAllDynamicTextLabels( );
```

Each textlabel has it's own set of methods and properties that can be used:
```csharp
// Get/set textlabel's position.
Vector3 Position { get; set; }

// Get/set scale of the textlabel.
int? Scale { get; set; }

// Get/set the textlabel's text.
string Text { get; set; }

// Get/set the textlabel centered.
bool Center { get; set; }

// Get/set the textlabel proportional.
bool Proportional { get; set; }

// Get/set the textlabel's color.
Rgba Color { get; set; }

// Get/set the textlabel's edge color.
Rgba Edge { get; set; }

// Get/set the textlabel's drop shadow.
DropShadow DropShadow { get; set; }

// Get/set the textlabel's font type.
int Font { get; set; }

// Destroy the textlabel and all it's data.
void Destroy( );
```

## Examples
```csharp
// Create a textlabel.
DynamicTextLabel textLabel = TextLabelStreamer.CreateDynamicTextLabel( "Some Text", new Vector3( -879.655f, -853.499f, 19.566f ), 0, true, new Rgba( 255, 255, 255, 255 ) );

// Change textlabel text.
textLabel.Text = "Some other text";

// Change position.
textLabel.Position = new Position( 300f, 500f, 25f ); // Accepts both Vector3 and Position types.

// Set the textlabel's color
textLabel.Color = new Rgba( 25, 49, 120, 255 ); // random

// Destroy the textlabel
TextLabelStreamer.DestroyDynamicTextLabel( textLabel ); // has an overload method that accepts an ID instead of textlabel instance.
```

Furthermore, there's an example C# file included in the package, the example file can be found at ``server-scripts/ExampleServerTextLabels.cs``.
