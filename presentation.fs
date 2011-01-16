#! /usr/bin/gforth
\ here-allokation wird als fifo verwendet.
: copy ( addrdst addrsrc len -- addrdstend )
	over ( dst src len src ) + swap ( dst end src )
	+do ( dst+ )
		i ( dst+ src+ ) c@ ( dst+ chr )
		over ( dst+ chr dst+ ) c! ( dst+ ) 1+
	loop
;

\ 2-rot ist bei gforth anscheinend nicht definiert
: 2-rot { a1 a2 b1 b2 c1 c2 } c1 c2 a1 a2 b1 b2 ;

: term-size form ;
: term-height ( -- i ) term-size drop ;
: term-width ( -- i ) term-size nip ;

: isnewline? ( c -- i ) dup 10 = swap 13 = or ;
: isspace?   ( c -- i ) dup 9 = over 11 = or swap 32 = or ;
: anyspaces? ( c -- i ) dup isnewline? isspace? or ;

: clearwspace ( c -- c )
	dup
	case
	9  of 32 endof
	11 of 32 endof
	13 of 10 endof
		dup
	endcase nip
;

variable typewriter-delay
: typewriter-type ( addr len -- )
	typewriter-delay @ -rot
	over + swap +do
		dup ms i @ emit
	loop
	drop
;
50 typewriter-delay !

variable ptype-lenl \ Wieviele Zeichen bereits in dieser Zeile geschrieben wurden
defer ptype-type
' type is ptype-type
: ptype-word ( addrw addrc c -- addrc+1 )
	-rot \ c addrw addrc
	dup -rot over - ptype-type \ c addrc
	swap emit 1+ \ addrc+1
;
: ptype-init ( addr len lenm lenl -- addre lenm lenl addrw addre addr )
	{ addr len lenm lenl }  addr len +  lenm  lenl  addr  addr len +  addr
(	2over + \ addr len lenm lenl addre
	rot swap tuck 2-rot \ addre lenm addr len lenl addre
	2over drop rot swap \ addre lenm addr len addre lenl addr
	dup -rot 2-rot \ addre lenm lenl addrw addr len addre addr
	2nip \ addre lenm lenl addrw addre addr
)
;
: ptype-newline ( lenm lenl addrw addrc -- lenm 0 addrw )
	10 ptype-word \ lenm lenl addrw=addrc+1
	nip 0 swap \ lenm lenl=0 addrw
;
: ptype-space ( lenm lenl addrw addrc -- lenm lenl1 addrw )
	\ s\" is a space\n" type
	32 ptype-word \ lenm lenl addrw=addrc+1
	swap 1+ swap \ lenm lenl+=1 addrw
;
: ptype-anychar ( lenm lenl addrw addrc -- lenm lenl addrw addrc )
	2over <=
	if
		\ lenm lenl addrw addrc
		rot tuck over swap - \ lenm addrw lenl addrc addrc-lenl \ m w l c c-l
		2over drop >= \ lenm addrw lenl addrc addrc-lenl>=addrw
		if \ Wort ist laenger als eine Zeile -> muss umgebrochen werden.
			1- -rot 1- -rot 2dup - \ lenm lenl addrc-1 addrw addrc-1-addrw
			ptype-type ." -" nip 1 swap dup \ lenm lenl addrw=addrc-1 addrc-1
		else \ Word erst in der naechsten Zeile ausgeben.
			nip 2dup - negate -rot \ lenm addrc-addrw addrw addrc
		then
		10 emit
	then
	rot 1+ -rot
;
: ptype' ( addre lenm 0 addrw addre addr -- )
	\ addre ist fuer die schleife unwichtig
	+do \ lenm lenl addrw
		i dup c@ \ lenm lenl addrw addrc c
		clearwspace
		case \ lenm lenl addrw addrc c
		10 of ptype-newline endof
		32 of ptype-space endof
		drop ptype-anychar
		endcase
	loop \ addre lenm lenl addrw
	over ptype-lenl !
	nip nip tuck - ptype-type
;
: ptype ( addr len -- ) term-width ptype-lenl @ ptype-init ptype' ;
: ptype-reset ( -- ) 0 ptype-lenl ! ;
ptype-reset \ ptype-lenl sollte von Anfang an 0 sein

: escape ( -- addr len ) s\" \e" ;
: csi ( -- addr len ) s\" \e[" ;
: sgr ( u -- ) csi type 0 0 d.r 109 ( m ) emit ;
: beep
	0 term-height 2 - at-xy
	7 emit s" *beep* not" type
;

\ Es folgen ein paar syntaktische Textauszeichnungen.
: {i}   ( addr -- addr )  7 sgr ;
: <i>   ( -- ) ['] {i}  , ;
: {/i}  ( addr -- addr ) 27 sgr ;
: </i>  ( -- ) ['] {/i} , ;
: {b}   ( addr -- addr )  1 sgr ; \ bold
: <b>   ( -- ) ['] {b}  , ;
: {/b}  ( addr -- addr ) 22 sgr ;
: </b>  ( -- ) ['] {/b} , ;
: {u}   ( addr -- addr )  4 sgr ; \ underline
: <u>   ( -- ) ['] {u}  , ;
: {/u}  ( addr -- addr ) 24 sgr ;
: </u>  ( -- ) ['] {/u} , ;
: {fc}  ( addr -- addr ) dup @ 30 + sgr cell+ ; \ frontcolor
: <fc>  ( -- ) ['] {fc} , , ;
: {/fc} ( addr -- addr ) 39 sgr ;
: </fc> ( -- ) ['] {/fc} , ;
: {bc}  ( addr -- addr ) dup @ 40 + sgr cell+ ; \ backgroundcolor
: <bc>  ( -- ) ['] {bc} , , ;
: {/bc} ( addr -- addr ) 49 sgr ;
: </bc> ( -- ) ['] {/bc} , ;
: {br}  ( addr -- addr ) cr ptype-reset ;
: <br>  ( -- , xt-{br} ) ['] {br} , ;
: {animation}  ( addr , xt -- addr ) dup @ ['] ptype-type defer! cell+ ;
: <animation>  ( xt -- addr u- , xt-{animation} xt ) ['] {animation} , , ;
: {/animation} ( addr -- addr ) ['] type is ptype-type ;
: </animation> ( -- , xt-{/animation} ) ['] {/animation} , ;
: <tw>  ( -- , xt xt ) ['] typewriter-type <animation> ;
: </tw> ( -- , xt ) </animation> ;
\ Es folgen ein paar blockorientierte Kennzeichnungen.
: {h}   ( addr , len -- addr )
	cr
	term-width over @ - 2 / \ addr width-twidth/2
	dup ptype-lenl !
	1 +do 32 emit loop
	cell+
	{b}
; \ header
: <h>   ( -- addr 0 , xt-{h} 0 )  ['] {h}  , here 0 , 0 ;
: {/h}  ( addr , len -- addr1 )
	{/b}
	cr
	term-width over @ - 2 / \ addr width-twidth/2
	dup
	2 +do 32 emit loop
	ptype-lenl @ swap 2 - +do 61 ( = ) emit loop
	cr
	cell+
;
: </h>  ( addr len -- , xt-{/h} len0 ) ['] {/h} , dup , swap ! ;
: {p}   ( addr -- addr ) cr ptype-reset cell+ ; \ paragraph
: <p>   ( -- addr u0 , xt-{p} 0 ) ['] {p}  , here 0 , 0 ;
: {/p}  ( addr -- addr ) cr ;
: </p>  ( addr len -- , xt-{/p} ) ['] {/p} , swap ! ;
: {li}  ( addr -- addr )
	s\"   * " type
	4 ptype-lenl !
	cell+
;
: <li>  ( -- addr u0 , xt-{li} 0 ) ['] {li} , here 0 , 0 ;
: {/li} ( addr -- addr ) cr ;
: </li> ( addr len -- , xt-{/li} ) ['] {/li} , swap ! ;

variable enumerationCount ( -- addr )
: {||}  ( addr -- addr )                \ increments enumeration count and prints prefix
        enumerationCount dup @ 1+ dup rot !
        s"   " type 0 U.R s" ) " type
	5 ptype-lenl !
;
: <||>  ( -- , xt-{||} 0 ) ['] {||} ,  ;
: {/||} ( addr -- addr ) cr ;
: </||> ( -- , xt-{/||} ) ['] {/||} , ;

: {en}  ( -- )  0 enumerationCount ! ;   \ resets enumeration count
: <en>  ( -- , xt-{en}  )  ['] {en} , ;
: {/en} ( -- ) ;
: </en> ( -- , xt-{/en} )  ['] {en} , ;


: {np} ( -- )
	0 sgr \ Alle Bildschirmeigenschaften zuruecksetzen
	page \ Bildschirm leeren
;
: {/np} ( -- )
	\ 30 sgr 40 sgr
	0 term-height 2 - at-xy
;
: <np> ( -- addr , xt-{/np} xt-{np} )
	\ Wir legen jede Anfangsadresse einer Seite auf den Stack (Achtung, in umgekehrter Reihenfolge)
	['] {/np} ,
	here
	['] {np} ,
;

: {!!} ( addr -- addr+2 )
	dup @   \ addr straddr
	swap    \ straddr addr
	cell+   \ straddr addr
	tuck    \ addr straddr addr
	@       \ addr straddr strlen
	ptype   \ addr
	cell+
;
: !! ( len0 addr1 len1 -- len , xt-{!!} addr1 len1 )
	['] {!!} ,
	dup \ len0 addr1 len1 len1
	rot , , \ len0 len1 len1 addr1 -> len0 len1
	+ \ len0+len1
;
:noname ( -- ) 34 parse save-mem !! ; :noname 34 parse postpone sliteral postpone !! ; interpret/compile: !"
:noname ( -- ) \"-parse save-mem !! ; :noname \"-parse postpone sliteral postpone !! ; interpret/compile: !\"

: pres_page_cur ( addr -- addr ) ;
: pres_page_from ( addr -- addr ) cell+ ;
: pres_page_to ( addr -- addr ) 2 cells + ;

\ faddr: erste Seitenzeigeradresse (letzte Presentationsseite)
\ laddr: letzte Seitenzeigeradresse (erste Presentationsseite)
\ paddr: derzeitige Seitenzeigeradresse

: page_steps ( laddr paddr 0 [u] -- u )
	\ u muss ungleich 0 sein. falls u nicht vorhanden: 1
	dup 0= if 1 then \ laddr paddr 0 u
	nip
;
: validpage? ( faddr laddr paddr -- faddr laddr paddr u )
	2dup <= \ faddr laddr paddr u
	2over drop rot tuck >
	rot \ faddr laddr paddr u u
	if drop cell - 1 \ faddr laddr paddr-1 -1 \ paddr-overflow
	else if cell+ -1 \ faddr laddr paddr+1 -1 \ paddr-underflow
	else 0 \ faddr laddr paddr 0
	then then
;
: showpage' ( paddr -- )
	dup cell - \ paddr paddr+cell
	@ swap @ \ naddr addr \ Seiteninhaltsadressen
	begin 2dup >
	while
		dup cell+ swap \ naddr xtaddr xtaddr
		@ \ naddr xtaddr xt
		execute \ verschiebt eventuell den Zeiger noch weiter, wenn es Parameter erwartet.
	repeat
	2drop
;
: showpage ( faddr laddr paddr -- faddr laddr paddr0 0 )
	validpage? 0 tuck 2-rot drop \ i 0 faddr laddr paddr0
	dup showpage' 0 2rot drop \ faddr laddr paddr0 0 i
	if beep then
;
: n ( faddr laddr paddr 0 [u] -- faddr laddr paddr 0 ) page_steps cells - showpage ;
: g ( faddr laddr paddr 0  u  -- faddr laddr paddr 0 ) cells nip nip over swap - showpage ;
: p ( faddr laddr paddr 0 [u] -- faddr laddr paddr 0 ) page_steps cells + showpage ;
: u ( faddr laddr paddr X     -- faddr laddr paddr 0 ) drop showpage ;
: q bye ;

: <presentation> ( -- addr0 0 addr1 , xt-{np} ) here 0 here ['] {np} , ;
: </presentation> ( 0 <addr...> -- faddr laddr paddr 0 !! endaddr 0 0 0 0 <...addr> )
	<np>
	here \ 0 <addr...> faddr
	begin swap dup \ 0 <addr..> addr0 faddr
	while , \ 0 <addr..> faddr
	repeat \ .s cr
	drop \ faddr
	here dup 0 \ faddr laddr paddr 0
	\ u \ Praesentation starten
;
