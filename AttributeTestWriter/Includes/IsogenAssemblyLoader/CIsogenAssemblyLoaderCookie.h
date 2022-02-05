#pragma once
#include "CIsogenAssemblyLoader.h"

#ifdef COMPILING_ISOGEN_ASSEMBLY_LOADER
#define DllExport   __declspec( dllexport )  
#else
#define DllExport   __declspec( dllimport ) 
#endif

namespace Intergraph
{
	namespace PersonalISOGEN
	{
		enum CActivationOptions
		{
			ManifestOnly,
			AssemblyResolveOnly,
			Both
		};

		class DllExport CIsogenAssemblyLoaderCookie //: IDisposable
		{

		protected:
			ULONG_PTR _cookie;
			CIsogenAssemblyLoader* pLoader;
			bool _arAdded;

		public:
			CIsogenAssemblyLoaderCookie(CIsogenAssemblyLoader * loader, CActivationOptions actOption);
			CIsogenAssemblyLoaderCookie(CIsogenAssemblyLoader * loader);
			~CIsogenAssemblyLoaderCookie();

		private:
			bool Activate();
			void Deactivate();
		};
	}
}