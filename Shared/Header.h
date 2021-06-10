#pragma once

#include <filesystem>
namespace fsystem = std::filesystem;


#ifdef _DEBUG
#define IMPORT_CONFIG Debug
#else
#define IMPORT_CONFIG Release
#endif 

#ifdef _WIN64
#define IMPORT_PLATFORM bin x64
#else
#define IMPORT_PLATFORM bin
#endif


#import "D:\\Git\\bin\\Debug\\Core Components\\ImportExport.dll" no_auto_exclude
#import "D:\\Git\\bin\\Debug\\Core Components\\POD.dll" no_auto_exclude


