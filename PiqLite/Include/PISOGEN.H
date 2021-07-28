/*----------------------------------------------------------------------+
| Copyright (1996) Alias Ltd., All rights reserved.                     |
|                                                                       |
| ISOGEN and Personal ISOGEN are trademarks of Imperial Chemical        |
| Industries PLC and Alias Ltd.                                         |
+----------------------------------------------------------------------*/
/*----------------------------------------------------------------------+
| Workfile:   pisogen.h                                                 |
| Created:    29 Jul 1996                                               |
|                                                                       |
| Change History:-                                                      |
|                                                                       |
| DB - 17/10/96 - Brought under change control                          |
| DB - 17/10/96 - For writing of banner to message file, added       	|
|                 message types 21-24, PLATFORM and PISOGEN_VERSION     |
| DB - 30/01/96 - Changed ISOGEN_HANDSHAKE_FAILED (from 4 to 8)         |
|               - New PISOGEN_VERSION (2.3.0)                           |
| DB - 30/07/97 - New PISOGEN_VERSION (2.3.1)                           |
| DB - 16/02/98 - Added #define's for SUCCESS and ERROR (basedefs.h no  |
|                 longer present under VC++ Version 5)                  |
| DB - 18/02/98 - Added I-Gen error codes                               |
| DB - 27/02/98 - New PISOGEN_VERSION (2.4.0)                           |
| DB - 24/04/98 - New PISOGEN_VERSION (3.0.0)                           |
| DB - 28/07/98 - New PISOGEN_VERSION (3.1.0)                           |
| DB - 06/08/98 - Put into SourceSafe (see there for later history)     |
| MEH -25/02/99 - Merge with pisodata.h and add some more error codes   |
| MEH -12/04/99 - New PISOGEN_VERSION (4.0.0)                           |
| IAN -10/01/00 - New PISOGEN_VERSION (4.2.0)                           |
| IAN -11/03/02 - New PISOGEN_VERSION (5.0.1)                           |
+----------------------------------------------------------------------*/

#ifndef	    __pisogenh__
#define	    __pisogenh__

/* Message types */
#define ISO_OK 0
#define PISOGEN_NOT_SET 1
#define INVALID_ISOTYPE 2
#define NO_PROJECT 3
#define IDFGEN_HANDSHAKE_FAILED 4
#define NO_PCF 5
#define ISOGEN_GET_CURRENT_FAILED 6
#define ISOGEN_CD_PROGRAMS_FAILED 7
#define ISOGEN_HANDSHAKE_FAILED 8
#define ISOGEN_FAILED 9
#define ISOGEN_CD_BACK_FAILED 10
#define IDFGEN_GET_CURRENT_FAILED 11
#define IDFGEN_CD_PROGRAMS_FAILED 12
#define IDFGEN_FAILED 13
#define IDFGEN_CD_BACK_FAILED 14
#define DEL_036_FAILED 15
#define CREATE_IDX_FAILED 16
#define COPY_FLS_FAILED 17
#define ISOGEN_AND_CD_BACK_FAILED 18
#define PISOGEN_HANDSHAKE_FAILED 19
#define UNKNOWN_ERROR 20
#define PISOGEN_GET_CURRENT_FAILED 21
#define PISOGEN_CD_PROGRAMS_FAILED 22
#define PISOGEN_WRITE_BANNER_FAILED 23
#define PISOGEN_CD_BACK_FAILED 24
#define IGEN_INITIALISE_FAILED 25
#define PISOGEN_INVALID_ROOT 26
#define PISOGEN_SETENV_FAILED 27
#define INVALID_FILE_EXTENSION 28
#define IDFGEN_PCF_FILE_REQUIRED 29

#define FILE_DOES_NOT_EXIST				60
#define ISOGEN_IDF_OR_DAT_FILE_REQUIRED 61
#define ISOA2B_FAILED					62
#define IDFGEN_UNITS_NOT_SET			63
#define IDFGEN_LIMIT_EXCEEDED			64

#define ISOGEN_ERROR					-1
#define ISOGEN_CRASH					-2
#define ISOGEN_TIMEOUT					-3
#define ISOGEN_THREAD_CREATION_FAILURE	-4
#define ISOGEN_LOAD_FAILURE				-5
#define IDFGEN_LOAD_FAILURE				-6
#define IDFGEN_CRASH					-7
#define IDFGEN_TIMEOUT					-8
#define IDFGEN_THREAD_CREATION_FAILURE	-9

#define EXTERNAL_PROCESS_CREATION_FAILURE				-1001
#define EXTERNAL_PROCESS_CRASH							-1002
#define EXTERNAL_PROCESS_TIMEOUT_TERMINATION_SUCCESS	-1003
#define EXTERNAL_PROCESS_TIMEOUT_TERMINATION_FAILURE	-1004

#define POD_PROCESS_WARNING				30
#define POD_IDFTRANS_PROCESS_FAILED		31
#define POD_PODTRANS_PROCESS_FAILED		32
#define POD_PODGRAPHICS_LOAD_FAILED		33
#define POD_PODGRAPHICS_PROCESS_FAILED	34
#define POD_PODTRANS_NO_INTELLIGENCE	35
#define POD_PODGRAPHICSPROCESSOR_FAILED	36

#define PISOGEN_GET_TEMP_PATH_FAILED	41
#define PISOGEN_TEMP_PATH_NOT_EXIST		42
#define PISOGEN_SETBOREUNITS_FAILED		43
#define PISOGEN_FLS_UPDATE_FAILED		44
#define PISOGEN_FLS_RESTORE_FAILED		45
#define PISOGEN_GET_MESSAGE_ENTRY_FAILED	46
#define PISOGEN_GET_OPL_ENTRY_FAILED		47
#define COPY_OPL_FAILED						48
#define PISOGEN_UPDATE_FLS_ENTRY_FAILED		49

#define	PROJECT_DISK_FULL				50
#define	TEMP_DISK_FULL					51
#define	PISOGEN_GETBOREUNITS_FAILED		52
#define COPY_FILE_FAILED				53
#define PISOGEN_GET_WINDOWS_DIRECTORY_FAILED	54
#define PISOGEN_GET_SYSTEM_DIRECTORY_FAILED		55

#define Err_PersonalIsogenFilesMissing   1000
#define Err_InconsistentUnitsCombination 1001
#define Err_InconsistentDrawingFormat    1002
#define Err_CannotDeleteStdOutFile       1003
#define Err_InvalidLineInOptionsFile     1004
#define Err_TooManySwitchesInOptionsFile 1005
#define Err_TooFewSwitchesInOptionsFile  1006
#define Err_MS_EXE_not_defined           1007
#define Err_USTATION_not_installed       1008
#define Err_MS_EXE_without_slash         1009
#define Err_PersonalIsogenFLSMissing     1010
#define Err_PersonalIsogenOPTSMissing    1011

#define ISOGEN_DRAWING_FAILED			 1999
#define PISOGEN_INCOMPATIBLE_UNITS		 2001
#define IDFGEN_DISCONNECTED_PIPELINE	 2022
#define IDFGEN_DISCONNECTED_PIPELINE_UNCONNECTED	 2023
#define IDFGEN_DISCONNECTED_PIPELINE_RESOLVED		 2024

//#define IGEN_OUTPUT_RENAME_FAILED 26

/* Other definitions */
#include "encrypt.h"

#ifndef MAX_PATH
#define MAX_PATH 256
#endif

#define MAX_RETURN_CODE_LENGTH 30

/* definitions needed from basedefs.h*/
#define SUCCESS		0x0000

#ifndef ERROR
#define ERROR		0x8000
#endif

/* Pisogen environmental variable */
#define PISOGEN_ENV _T("PISOGEN")

/* Banner information */
#define PLATFORM _T("INTEL NT")

#define MAX_DRAWINGS 256

#define MAX_PISOGEN_ROOT		80
#define MAX_PISOGEN_PROJECT		80
#define MAX_PISOGEN_STYLE		80
#define MAX_PISOGEN_ERRORTEXT	255
#define MAX_PISOGEN_MESSAGEFILE	255
#define MAX_PISOGEN_FILENAMES	255 * MAX_DRAWINGS

/* Main Pisogen data structure used to specify and return information */

/* ANSI Structure */
typedef struct
{
    char    pisogenRoot[MAX_PISOGEN_ROOT];			// root directory - including a trailing slash
    char    pisogenProject[MAX_PISOGEN_PROJECT];	// project name
    char    pisogenStyle[MAX_PISOGEN_STYLE];		// style of iso to be produced
    int     dimUnits;								// dimensional units (M, MM or FT-IN
    int     boreUnits;								// bore units (MM or IN)
    int     weightUnits;							// weight units (KG or LB)
    int     drawingFormat;							// format of plot file (DXF or DGN)
    int     viewPoint;								// drawing viewpoint (TL, TR, BL or BR)
    int     tolerance;								// IDFGEN / ISOGEN tolerance control (mm)
    int     messageLevel;							// IDFGEN / ISOGEN message level
	int     demoMode;								// Toggle for isometric demo mode
	char    errorText[MAX_PISOGEN_ERRORTEXT];		// (out) message text returned by ISOGEN
    char    messageFile[MAX_PISOGEN_MESSAGEFILE];	// (out) name of message file
    int     nFiles;									// (out) number of files generated by ISOGEN
    char    fileNames[MAX_PISOGEN_FILENAMES];
													// (out) array of file names created containing drawings generated by ISOGEN
													// The list contains a maximum of MAX_DRAWINGS files. Space has been malloced
													// for each of the nFiles filenames in the list, and should be freed after use
} PisogenData;

/* Number of switches in options file */
#define N_OPTION_SWITCHES 140

/* Dimensional units */
#define DIM_UNITS_M 0
#define DIM_UNITS_MM 1
#define DIM_UNITS_FTIN 2
#define DIM_UNITS_USER 3

/* Bore units */
#define BORE_UNITS_MM 4
#define BORE_UNITS_IN 8
#define BORE_UNITS_USER 12

/* Weight units */
#define WEIGHT_UNITS_KG 16
#define WEIGHT_UNITS_LB 32
#define WEIGHT_UNITS_USER 48

/* Drawing format */
#define ISOGEN_FORMAT 100
#define DXF_FORMAT 104
#define DGN_FORMAT 108
#define SMART_IGR_FORMAT 110
#define SMART_DXF_FORMAT 111
#define SMART_DGN_FORMAT 112
#define SMART_DWG_FORMAT 113
#define POD_FORMAT 114
#define SMART_SHA_FORMAT 115
#define USER_FORMAT 116
#define SMART_DGN8_FORMAT 117

/* Drawing viewpoint */
#define VIEW_TL 200
#define VIEW_TR 201
#define VIEW_BL 202
#define VIEW_BR 203
#define VIEW_USER 204

/* Message level */
#define MSG_LEVEL_USER 300
#define MSG_LEVEL_LOW 301
#define MSG_LEVEL_MED 302
#define MSG_LEVEL_HIGH 303

#define MAXLINELENGTH 1024
#define MAXFILELENGTH 255
#define PISO_SUCCESS  0

#define FALSE 0
//#define min 0

/* Personal ISOGEN modes */
#define PISO_MODE_DEMO 0
#define PISO_MODE_FULL 1
#define PISO_MODE_LIMITED 2

/* Returns while reading FLS */
#define FLS_BLANK_LINE 1
#define FLS_COMMENT_LINE 2
#define FLS_FORMAT_ERROR 3

/* Demo mode options */
#define PISO_SET_DEMO_MODE		1
#define PISO_SET_SPOOLING_OFF	2

/* Error codes */


#endif
