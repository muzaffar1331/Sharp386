# Sharp386
Sharp386 is a simplified version of the Intel 80386 processor instruction set written in C#.  Sharp386 is not a full-fledged virtual machine; it's intended to be a learning tool for x86 Assembly language, and meant to be used when testing x86 Assembly code. Sharp386 is 
a work in progress and as of the time of writing, only a few instructions are supported (see below). 

Current plans include implementing mostly used opcodes like MOV, ADD, SUB, INC, DEC, MUL, DIV, CALL, RET, PUSH, POP, OR, AND, NOT, XOR as well as support for 16-bit and 32-bit mode.

## Supported Instructions
The following instructions have been implemented, and are working in both 16-bit and 32-bit mode.

MOV r8, r8
MOV r16, r16

MOV r8, imm8/m16/m32  
MOV r16, imm16/m16/m32  
MOV r32, imm32/m16/m32  

INC r8  
INC r16  
INC r32  

INC byte m16/m32  
INC word m16/m32  
INC dword m16/m32  

NOP

JMP rel8

## References

Intel x86 Assembler Instruction Set Opcode Table
http://sparksandflames.com/files/x86InstructionChart.html

x86 Instruction Set Reference
https://c9x.me/x86/
