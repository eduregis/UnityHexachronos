-> main

=== main ===
[Cap_Sad] Olá, sou mais um diálogo!
[Thunder_Sigh] E eu sou outro diálogo!
[Cap] Estou funcionando!
Espero...
[Thunder_Sigh] Ei, que time é teu?
    + [Flamengo]
        -> chosen("Flamengo")
    + [Fluminense]
        -> chosen("Fluminense")
    + [Bateu na trave e entrou no teu]
        -> aff

=== chosen(time) ===
Aqui é {time}!
-> END

=== aff ===
Aff...
-> END