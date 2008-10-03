#include <iostream>

#define cpuid(index,eax,ebx,ecx,edx)\
    asm volatile\
        ("mov %%ebx, %%esi\n\t"\
         "cpuid\n\t"\
         "xchg %%ebx, %%esi"\
         : "=a" (eax), "=S" (ebx), "=c" (ecx), "=d" (edx)\
         : "0" (index));


//	asm(".intel_syntax noprefix\n");
//	asm("mov eax,0\n");
//	asm("cpuid\n");
//	asm("mov dword ptr [VendorID],ebx\n");
//	asm("mov dword ptr [VendorID+4],edx\n");
//	asm("mov dword ptr [VendorID+8],ecx\n");
//	asm("mov byte ptr [VendorID+12],0\n");

int main()
{
    int eax, ebx, ecx, edx;
    int max_std_level;

    cpuid(0, max_std_level, ebx, ecx, edx);

    std::cout << "max_std_level: '" << max_std_level << "'" << std::endl;
    std::cout << "ebx: '" << ebx << "'" << std::endl;
    std::cout << "ecx: '" << ecx << "'" << std::endl;
    std::cout << "edx: '" << edx << "'" << std::endl;

    std::cin.sync();
    std::cin.get();
    return 0;
}
