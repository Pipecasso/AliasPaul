#pragma once
#include <string>
#include <windows.h>

#ifdef COMPILING_ISOGEN_ASSEMBLY_LOADER
#define DllExport   __declspec( dllexport )  
#else
#define DllExport   __declspec( dllimport ) 
#endif

namespace Intergraph
{
	namespace PersonalISOGEN
	{
		class DllExport CIsogenAssemblyLoader 
		{
		public:
			enum ErrorType
			{
				None = 0,
				InvalidRegKey = 10,
				InvalidValue = 20,
				InvalidPath = 30,
				UnexpectedError = 100
			};

			struct ManagedIsogenAssemblyLoader;
			ManagedIsogenAssemblyLoader * _handleIsogenAssemblyLoader;

			CIsogenAssemblyLoader(wchar_t* Manifest, wchar_t* ManifestRoot, wchar_t* pSearchPath = nullptr, bool searchAllDirectories = false);
			CIsogenAssemblyLoader(wchar_t* pSearchPath, bool searchAllDirectories = false);

			CIsogenAssemblyLoader(char* Manifest, char* ManifestRoot, char* pSearchPath = nullptr, bool searchAllDirectories = false);
			CIsogenAssemblyLoader(char* pSearchPath, bool searchAllDirectories = false);
			~CIsogenAssemblyLoader();

			HANDLE GetHandle();
			void AddAssemblyResolve();
			void RemoveAssemblyResolve();
			
			wchar_t* GetErrorMessageW();

			char* GetErrorMessageA();

			bool AddStringPath(wchar_t* pathToAdd);
			bool RemoveStringPath(wchar_t* pathToRemove);
			ErrorType AddPathFromRegKey(wchar_t* LocalRegKeyPath, wchar_t* addendum = nullptr, wchar_t* Value = nullptr);
			bool RemovePathFromRegKey(wchar_t* LocalRegKeyPath, wchar_t* addendum = nullptr, wchar_t* Value = nullptr); 

			bool AddStringPath(char* pathToAdd);
			bool RemoveStringPath(char* pathToRemove);
			ErrorType AddPathFromRegKey(char* LocalRegKeyPath, char* addendum = nullptr, char* Value = nullptr);
			bool RemovePathFromRegKey(char* LocalRegKeyPath, char* addendum = nullptr, char* Value = nullptr); 

		private:
			wchar_t* CharToWchar(char* sourceString);
			wchar_t* _defaultAddendum;
			wchar_t* _defaultPath;
			HANDLE _hContext;
		};
	}
}
