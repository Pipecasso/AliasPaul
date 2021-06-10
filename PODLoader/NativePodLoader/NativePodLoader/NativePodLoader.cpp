// NativePodLoader.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "LoadedPOD.h"
#include "CIsogenAssemblyLoaderCookie.h"

int main()
{
    std::cout << "Hello World!\n";
    tstring manifest(_T("D:\\IRUXGit\\bin\\Debug\\I-Configure.exe.manifest"));
    tstring pod(_T("D:\\PODPCFStore\\StopYoureMessingAround\\CR_18343_1.POD"));

    LoadedPOD lp(pod, manifest);
    IPI::CIsogenAssemblyLoaderCookie* monster = new IPI::CIsogenAssemblyLoaderCookie(lp.GetIsogenAssemblyLoader());

    AliasPOD::IPipelinePtr p = lp.GetPod()->Pipelines->Item(0l);
    AliasPOD::IAttributesPtr patts = p->Attributes;

    delete monster;
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
