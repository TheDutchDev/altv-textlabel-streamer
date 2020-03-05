/*
    Developed by DasNiels/Niels/DingoDongBlueBalls
    Async Models loading by Micaww
*/

import * as alt from 'alt';

import { textLabelStreamer } from "./textlabel-streamer";

// when an object is streamed in
alt.onServer( "entitySync:create", entity => {
    if( entity.data ) {
        let data = entity.data;

        if( data && data.entityType === "3dtextlabel" ) {

            // alt.log( 'streamin texltabel: ', JSON.stringify( entity ) );

            textLabelStreamer.addTextLabel(
                +entity.id, data.text, entity.position, data.scale, data.font, data.color, data.dropShadow, data.edge, data.center, data.proportional, data.entityType
            );
        }
    }
    // this entity has streamed in before
    // else
    // {
    //     textLabelStreamer.restoreTextLabel( +entity.id );
    // }
} );

// when an object is streamed out
alt.onServer( "entitySync:remove", entityId => {

    //alt.log( 'streamout: ', entityId );
    textLabelStreamer.removeTextLabel( +entityId );
} );

// when a streamed in object changes position data
alt.onServer( "entitySync:updatePosition", ( entityId, position ) => {
    let textLabel = textLabelStreamer.getTextLabel( +entityId );

    if( textLabel === null )
        return;

    textLabelStreamer.setPosition( textLabel, position );
} );

// when a streamed in object changes data
alt.onServer( "entitySync:updateData", ( entityId, newData ) => {
    let textLabel = textLabelStreamer.getTextLabel( +entityId );

    if( textLabel === null )
        return;

    if( newData.hasOwnProperty( "center" ) )
        textLabelStreamer.setCenter( textLabel, newData.center );

    if( newData.hasOwnProperty( "color" ) )
        textLabelStreamer.setColor( textLabel, newData.color );

    if( newData.hasOwnProperty( "center" ) )
        textLabelStreamer.setDropShadow( textLabel, newData.center );

    if( newData.hasOwnProperty( "edge" ) )
        textLabelStreamer.setEdge( textLabel, newData.edge );

    if( newData.hasOwnProperty( "font" ) )
        textLabelStreamer.setFont( textLabel, newData.font );

    if( newData.hasOwnProperty( "proportional" ) )
        textLabelStreamer.setProportional( textLabel, newData.proportional );

    if( newData.hasOwnProperty( "scale" ) )
        textLabelStreamer.setScale( textLabel, newData.scale );

    if( newData.hasOwnProperty( "text" ) )
        textLabelStreamer.setText( textLabel, newData.text );

} );

// when a streamed in object needs to be removed
alt.onServer( "entitySync:clearCache", entityId => {
    textLabelStreamer.clearTextLabel( +entityId );
} );