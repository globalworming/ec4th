\ constants for AVR io registers

HEX

\ ################################################REGISTERS#####################
\ Ports
\ DDRx = Data Direction Register X
  \ Port A
    00 constant PINA
    01 constant DDRA
    02 constant PORTA
  \ Port B
    03 constant PINB
    04 constant DDRB
    05 constant PORTB
  \ Port C
    06 constant PINC
    07 constant DDRC
    08 constant PORTC
  \ Port D
    09 constant PIND
    0A constant DDRD
    0B constant PORTD


\ Stack-Pointer
  3D constant SPL \ Low
  3E constant SPH \ High


\ Status Register
  3F constant SREG


\ Asynchronous Status Register
  B6 constant ASSR


\ EEPROM Registers
  21 constant EEARL \ EEPROM Address Register Low
  22 constant EEARH \         -"-             High
  1F constant EECR  \ EEPROM Control Register
  20 constant EEDR  \ EEPROM Data Register


\ General Purpose I/O Registers 0-2
  1E constant GPIOR0
  2A constant GPIOR1
  2B constant GPIOR2


\ SMCR - Sleep Mode Control Register
  33 constant SMCR


\ MCU
  34 constant MCUCR \ MCUCR - MCU Controll Register
  35 constant MCUSR \ MCUSR - MCU Status Register


\ PRR - Power Reduction Register
  64 constant PRR


\ Watchdog
  60 constant WDTCSR \ Watchdog Timer Control Register


\ External Interrupts
  69 constant EICR  \ External Interrupt Control Register A
  1C constant EIFR  \ External Interrupt Mask Register
  1D constant EIMSK \ External Interrupt Flag Register
  \ Pin Change Interrupts
    68 constant PCICR  \ Pin Change Interrupt Control Register
    1B constant PCIFR  \ Pin Change Interrupt Flag Register
    \ Pin Change Mask Registers 0-2
      6B constant PCMSK0
      6C constant PCMSK1
      6D constant PCMSK2


\ Timer/Counter Registers
  24 constant TCCR0A  \ Timer/Counter Control Register A
  25 constant TCCR0B  \ Timer/Counter Control Register B
  26 constant TCNT0   \ Timer/Counter Register
  \ Timer/Counter Interrupt Mask Registers 0-2
    6E constant TIMSK0
    6F constant TIMSK1
    70 constant TIMSK2
  \ Timer/Counter Interrupt Flag Register 0-2
    15 constant TIFR0
    16 constant TIFR1
    17 constant TIFR2
  \ Timer/Counter 1 Control Registers A-C
    80 constant TCCR1A
    81 constant TCCR1B
    82 constant TCCR1C
  \ Timer/Counter 1
    84 constant TCNT1L \ Low
    85 constant TCNT1H \ High
  \ Timer/Counter 2 Control Registers A/B
    B0 constant TCCR2A
    B1 constant TCCR2B
  \ Timer/Counter 2
    B2 constant TCNT2


\ General Timer/Counter Control Register
  23 constant GTCCR


\ Output Compare Registers
  \ Output Compare Registers 0
    27 constant OCR0A  \ Output Compare Register A
    28 constant OCR0B  \         -"-             B
  \ Output Compare Registers 1
    88 constant OCR1AL \ Output Compare Register A Low
    89 constant OCR1AH \            -"-            High
    8A constant OCR1BL \            -"-          B Low
    8B constant OCR1BH \            -"-            High
  \ Output Compare Registers 2
    B3 constant OCR2B \ Output Compare Register B
    B4 constant OCR2A \ Output Compare Register A


\ Input Capture Register 1
  86 constant ICR1L \ Low
  87 constant ICR1H \ High


\ Serial Peripheral Interface
  2C constant SPCR \ SPI Control Register
  2D constant SPSR \ SPI Status Register
  2E constant SPDR \ SPI Data Register


\ USART
  \ USART I/O Data Register
    C6 constant UDR0
  \ Userart Control and Status Registers A-C
    C0 constant UCSR0A
    C1 constant UCSR0B
    C2 constant UCSR0C
  \ USART Baud Rate Registers
    C4 constant UBRR0L \ Low
    C5 constant UBRR0H \ High


\ 2-Wire Serial Interface Registers
  B9 constant TWSR  \ TWI Status Register
  BB constant TWDR  \ TWI Data Register
  B8 constant TWBR  \ TWI Bit Rate Register
  BC constant TWCR  \ TWI Control Register
  BA constant TWAR  \ TWI (Slave) Address Register
  BD constant TWAMR \ TWI (Slave) Address Mask Register

\ Analog
  \ Analog Comparator
    \ Analog Comparator Control and Status Register
      30 constant ACSR
  \ Analog-to-Digital Converter
    \ ADC Control and Stauts Registers A/B
      7A constant ADCSRA
      7B constant ADCSRB
    \ ADC Data Register
      78 constant ADCL \ Low
      79 constant ADCH \ High
    \ ADC Multiplexer Selection Register
      7C constant ADMUX

\ Digital Input Disable Registers 0/1
  7E constant DIDR0
  7F constant DIDR1


\ Store Program Memory Control and Status Register
  37 constant SPMCSR


\ Clock Prescale Register
  61 constant CLKPR


\ Oscillator Calibration Register
  66 constant OSCCAL
\ ###################################################END########################

DECIMAL