// JavaScript source code
'use strict';

console.log('Loading function');

exports.handler = (SatelliteUpdateId, context, callback) => {
    console.log('Starting Lambda Method call!');

    console.log('SatelliteUpdateId =', SatelliteUpdateId.SatelliteUpdateId);

    console.log('Obtaining SDK...');    
    var AWS = require("aws-sdk");    
    console.log('SDK obtained!');

    console.log('Creating client...');    
    var docClient = new AWS.DynamoDB.DocumentClient();    
    console.log('Client created!');

    console.log('Setting up params...');
    var params = {        
    	TableName: "SatelliteUpdates",        
    	KeyConditionExpression: "SatelliteId = :satelliteUpdateId",        
    	ExpressionAttributeValues: {            
    		":satelliteUpdateId": Number(SatelliteUpdateId.SatelliteUpdateId)        
    	}    
    };   
    console.log('Params setup!');

    console.log('Running query...');		
    docClient.query(params, function(err, data) 
    {    
    	if (err)        
    		console.log(JSON.stringify(err, null, 2));    
    	else        
    	{
    		console.log(JSON.stringify(data, null, 2));
            context.done(null, data);
    	}
    });

    console.log('Lambda Method call complete!');
};
