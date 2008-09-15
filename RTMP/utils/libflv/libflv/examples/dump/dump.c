#include <flv.h>

int main(int argc, char *argv[]) {
	if(argc >= 1) {
		dumpFLVFile(argv[1]);
	}
	return 0;
}
