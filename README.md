# JunkLang: A Joke Language
A joke language being made for fun
# Description
Emulates a made up processor that functions as follows:
1 Integer Register
- Can contain any integer number, be written and read from freely. Most instructions will do direct work on this register
- Accessed as #IREG
1 Floating Point Register
- Can contain any floating point number, be written and read from freely. Most instructions will do direct work on this register
- Accessed as #FREG
1 Swap Only Registers
- Arithmetic and logic can not be performed on these registers. Their only purpose is to hold a value to later be swapped from.
- Accessed as #SWAP
1 Intermediary Register
- Arithmetic and logic can not be performed on this register, but the value inside can be used to perform arithmetic on the General Purpose Register.
- Accessed as #INTR
All registers can have their data changed by swapping values between them.
# Instructions
## Data Manipulation
SWAP [REG1] [REG2]
- Exchanges the values between the registers. [REG2] will take [REG1]’s value and vice versa.
COPY [REG1] [REG2]
- Replaced the value of [REG2] with the value of [REG1]
SETN [VALUE] [REG]
- Sets the value of [REG] to [VALUE]
## Arithmetic
ADDN [VALUE/REG1] [REG2]
- Adds the value of [VALUE/REG1] to [REG2] and stores it in [REG2]
SUBN [VALUE/REG1] [REG2]
- Subtracts the value of [VALUE/REG1] from [REG2] and stores it in [REG2]
MULN [VALUE/REG1] [REG2]
- Multiplies the value of [REG2] by [VALUE/REG1] and stores it in [REG2]
DIVN [VALUE/REG1] [REG2]
- Divides the value of [REG2] by [VALUE/REG1] and stores it in [REG2]
SUBR [VALUE/REG1] [REG2]
- Subtracts the value of [REG2] from [VALUE/REG1] and stores it in [REG2]
DIVR [VALUE/REG1] [REG2]
- Divides the value of [REG2] by [VALUE/REG1] and stores it in [REG2]
## Flow Control
LABL [NAME]
- Names a line to be returned to using the GOTO instruction
- Can not be a number
GOTO [LABL/LINE #]
- Resets execution to a specified line number or LABL
FUNC [NAME] [PARAMETER]
- Names a function that can be returned to using the CALL instruction
- Returns to the CALL instruction after reaching a DONE instruction
- Can pass either a numeric value or register as a parameter
CALL [FUNCTION]
- Sends execution to a specified function
One function had reached a DONE instruction, resume execution after the CALL
## Logic
IFEQ [VALUE1/REG1] [VALUE2/REG2]
- If [VALUE1/REG1] equals [VALUE2/REG2] execute until next DONE instruction
IFNQ [VALUE1/REG1] [VALUE2/REG2]
- If [VALUE1/REG1] does not equal [VALUE2/REG2] execute until next DONE instruction
IFGT [VALUE1/REG1] [VALUE2/REG2]
- If [VALUE1/REG1] is greater than [VALUE2/REG2] execute until next DONE instruction
IFLT [VALUE1/REG1] [VALUE2/REG2]
- If [VALUE1/REG1] is less than [VALUE2/REG2] execute until next DONE instruction

## Input/Output
OUTS [STRING]
- Outputs a given string
OUTR [REG]
- Outputs the value of a register
TAKE [REG]
- Takes a value from the user and stores it in [REG]
