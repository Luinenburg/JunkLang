namespace JunkLang.Machine;

public enum Instructions
{
    SWAP,
    COPY,
    SETN,
    
    ADDN,
    SUBN,
    MULN,
    DIVN,
    
    SUBR,
    DIVR,
    
    LABL,
    GOTO,
    FUNC,
    CALL,
    
    IFEQ,
    IFNQ,
    IFGT,
    IFLT,
    
    OUTS,
    OUTR,
    TAKE
}