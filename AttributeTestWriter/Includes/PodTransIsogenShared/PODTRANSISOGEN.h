
enum IsogenEventType
{
	ISOGEN_ARC,
	ISOGEN_CIRCLE,
	ISOGEN_ELLIPSE,
	ISOGEN_LINE,
	ISOGEN_POLYGON,
	ISOGEN_POLYLINE,
	ISOGEN_QUADRILATERAL,
	ISOGEN_TEXT,
	ISOGEN_TRIANGLE,
	ISOGEN_FONT,
	ISOGEN_LAYER,
	ISOGEN_LINESTYLE,
	ISOGEN_MAXPRIMITIVE = 1000,

	ISOGEN_KEYPOINT,
	ISOGEN_MESSAGESTART,
	ISOGEN_MESSAGEEND,
	ISOGEN_MAXMESSAGEKEYPOINT = 2000,

	ISOGEN_TUBESTART,
	ISOGEN_TUBEEND,
	ISOGEN_COMPONENTSTART, 
	ISOGEN_COMPONENTEND,
	ISOGEN_WELDSTART, 
	ISOGEN_WELDEND,
	ISOGEN_SPOOL,
	ISOGEN_CUTPIECE,
	ISOGEN_MAXCOMPONENT = 3000,

	ISOGEN_PODSTART, 
	ISOGEN_PODEND,
	ISOGEN_INITIALISE,
	ISOGEN_SETTINGS,
	ISOGEN_PROCESS,
	ISOGEN_MAXSYSTEM=4000,

	ISOGEN_DETAILSKETCHDIRECTORY,
	ISOGEN_DETAILSKETCH,
	ISOGEN_DETAILSKETCHPARAMETER,
	ISOGEN_DETAILSKETCHEND,
	ISOGEN_INFORMATIONNOTE,
	ISOGEN_MAXDETAILSKETCH = 5000,
	
	ISOGEN_BACKINGSHEET,
	ISOGEN_OUTPUTNAME,
	ISOGEN_MAXFILE = 6000,
};

struct IsogenText
{
	long font;
	char* text;
	long averageCharacterHeight;
	long averageCharacterWidth;
	long textAngle;
	long characterAngle;
	long justification;
};

struct IsogenLine
{
	long lineStyle;
	long lineWidth;
	long lineWeight;
	long number;
	char* name;
};

struct IsogenFill
{
	long colour;
	long backgroundColour;
	long hatchStyle;
};

struct IsogenCoord
{
	long x;	
	long y;	
	long z;
};

struct IsogenCoords
{
	IsogenCoord coord[4];
	long pointCount;
	long* xPoints;
	long* yPoints;
	long rotationAngle;
};

struct IsogenCircle
{
	long radiusX;
	long radiusY;
	long startAngle;
	long sweepAngle;
};

struct IsogenFont
{
	long number;
	long type;
	char* name;
	char* typeface;
	char* style;
};

struct IsogenLayer
{
	long number;
	long colour;
	char* name;
};

struct IsogenComponent
{
	long recordId;
	long sequenceId;
	char* sKey;
	char* partIdentifier; 
	long weight;
	long spoolSequenceNumber;    
	long spoolSheetNumber;
	long cutPieceSequenceNumber; 
	long cutPieceSheetNumber;
	long bore1;
	long bore2;
	long bore3;
	long flangeRotationAngle;
};

struct IsogenSpool
{
	long sequenceNumber;
	long sheetNumber;
	char* identifier; 
	long weight;
	long factorDiameter;
	char* serviceCommodityCode;
	long surfaceArea;
	long weldDiameter;
	long centrelineLength;
};

struct IsogenCutPiece
{
	long sequenceNumber;
	long sheetNumber;
	long spoolSequenceNumber;
	long spoolSheetNumber;
	char* identifier; 
	long weight;
	long length;
	char* endPrep1;
	char* endPrep2;
	char* remark;
};

struct IsogenWeld
{
	char* weldIdentifier;
	long beforeSequenceID;
	long afterSequenceID;
	long impliedWeldSequenceID;
	char* type;
	long typeNo;
};

struct IsogenMessage
{
	long type;
	long messageNumber;
	long masterMessageNumber;
	long messageSequenceId;
	long componentSequenceId;
	long impliedWeldSequenceId;
};

struct IsogenInitialise
{
	char* idFName;
	long sheetCount;
	long paperSize;
	long paperOrientation;
	long paperHeight;
	long paperWidth;
	long northArrow;
	long outputFormat;
	char* pipeReference;
	long pipeReferenceType;
	void *pIsogenSharedData;
};

struct IsogenSettings
{
	long option31;
	long option92Switch1;
};

struct IsogenDetailSketch
{
	char* name;
	long format;
	IsogenCoord coord;
	long width; 
	long height;
	long isogenSequenceNumber;
	char* parameter;
	char* operation;
	char* value;
};

struct IsogenFile
{
	char* name;
	long format;
};

struct IsogenPrimitiveEventArgs
{
	IsogenCoords Coords;
	IsogenLine Line;
	IsogenText Text;
	IsogenFill Fill;
	IsogenCircle Circle;
	IsogenFont Font;
	IsogenLayer Layer;
};

struct IsogenComponentEventArgs 
{
	IsogenCoords Coords;
	IsogenComponent Component;
	IsogenSpool Spool;
	IsogenCutPiece CutPiece;
	IsogenWeld Weld;
};

struct IsogenMessageKeypointEventArgs 
{
	IsogenCoords Coords;
	IsogenMessage Message;
};

struct IsogenSystemEventArgs
{
	IsogenInitialise Initialise;
	IsogenSettings Settings;
};

struct IsogenDetailSketchEventArgs
{
	IsogenDetailSketch DetailSketch;
};

struct IsogenFileEventArgs 
{
	IsogenFile File;
};

class CIsogenBaseEventArgs
{
public:
	long status;
};

class CIsogenPrimitiveEventArgs : public CIsogenBaseEventArgs
{
public:
	IsogenPrimitiveEventArgs IsogenEventArgs;
};

class CIsogenComponentEventArgs : public CIsogenBaseEventArgs
{
public:
	IsogenComponentEventArgs IsogenEventArgs;
};

class CIsogenMessageKeypointEventArgs : public CIsogenBaseEventArgs
{
public:
	IsogenMessageKeypointEventArgs IsogenEventArgs;
};

class CIsogenSystemEventArgs : public CIsogenBaseEventArgs
{
public:
	IsogenSystemEventArgs IsogenEventArgs;
};

class CIsogenFileEventArgs : public CIsogenBaseEventArgs
{
public:
	IsogenFileEventArgs IsogenEventArgs;
};

class CIsogenDetailSketchEventArgs : public CIsogenBaseEventArgs
{
public:
	IsogenDetailSketchEventArgs IsogenEventArgs;
};






