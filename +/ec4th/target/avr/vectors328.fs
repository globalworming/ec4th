\ Reset and interrupt vector address for the ATMEL 328 ;12mar26jaw

label jmp-reset 0 jmp, \ 0x0000: Reset Handler
label jmp-ext_int0 ret, \ 0x0002: IRQ0 Handler
label jmp-ext_int1 ret, \ 0x0004: IRQ1 Handler
label jmp-pcint0 ret, \ 0x0006: PCINT0 Handler
label jmp-pcint1 ret, \ 0x0008: PCINT1 Handler
label jmp-pcint2 ret, \ 0x000A: PCINT2 Handler
label jmp-wdt ret, \ 0x000C: Watchdog Timer Handler
label jmp-tim2_compa ret, \ 0x000E: Timer2 Compare A Handler
label jmp-tim2_compb ret, \ 0x0010: Timer2 Compare B Handler
label jmp-tim2_ovf ret, \ 0x0012: Timer2 Overflow Handler
label jmp-tim1_capt ret, \ 0x0014: Timer1 Capture Handler
label jmp-tim1_compa ret, \ 0x0016: Timer1 Compare A Handler
label jmp-tim1_compb ret, \ 0x0018: Timer1 Compare B Handler
label jmp-tim1_ovf ret, \ 0x001A: Timer1 Overflow Handler
label jmp-tim0_compa ret, \ 0x001C: Timer0 Compare A Handler
label jmp-tim0_compb ret, \ 0x001E: Timer0 Compare B Handler
label jmp-tim0_ovf ret, \ 0x0020: Timer0 Overflow Handler
label jmp-spi_stc ret, \ 0x0022: SPI Transfer Complete Handler
label jmp-usart_rxc ret, \ 0x0024: USART RX Complete Handler
label jmp-usart_udre ret, \ 0x0026: USART UDR Empty Handler
label jmp-usart_txc ret, \ 0x0028: USART TX Complete Handler
label jmp-adc ret, \ 0x002A: ADC Conversion Complete Handler
label jmp-ee_rdy ret, \ 0x002C: EEPROM Ready Handler
label jmp-ana_comp ret, \ 0x002E: Analog Comparator Handler
label jmp-twi ret, \ 0x0030: 2-wire Serial Interface Handler
label jmp-spm_rdy ret, \ 0x0032: Store Program Memory Ready Handler
end-label
