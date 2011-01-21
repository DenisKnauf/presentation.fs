: printCodeHeader ( end start namelen addr 1/0 -- )  \ prints source code header containing line numbers
  { showLines }
  swap 2swap \ addr namelen end start
	2dup > if swap then \ addr namelen start/end end/start
	showLines if dup 0 <# #s #> nip else 0 then ptype-reset 1+ ptype-indent !
  2swap \ start end addr namelen
	showLines printsource
;

: {source} ( -- ) dup dup dup dup @ swap cell + @  2swap cell 2 * +
		   @ swap cell 3 * + @ 1 printCodeHeader 4 cells + ptype-reset ;
: <source> ( addr len from to -- , xt-{source} )  ['] {source} , , , , , ;
