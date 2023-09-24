# Rinha de Compilers

Este compilador/interpretador será feito em C#

## Funcionalidades implementadas

- Int
- Str
- Call
- Binary
- Function
- Let
- If
- Print
- First
- Second
- Bool
- Tuple
- Var

Seguindo especificação da [rinha de compiladores](https://github.com/aripiprazole/rinha-de-compiler).

## Para executar

Navegue para a raiz do repositório.
Execute:

```
docker build -t rinha-image -f Dockerfile .
```

Após, execute: 
```
docker run -it --rm rinha-image 
```

## Para testar

Ao rodar o programa buscará pelo arquivo source.rinha.json dentro de /var/rinha. Porém é possível passar um parâmetro para o programa especificando o nome do arquivo desejado (dentro de  /var/rinha). Exemplo: 
```
docker run -it --rm rinha-image arquivo_desejado.json
```

Para testar passando o seu arquivo como exemplo e dentro das especificações da rinha, execute:
* Troque <FILE_PATH> pelo o endereço do seu arquivo AST em json.
```
docker run -v <FILE_PATH>:/var/rinha/source.rinha.json --memory=2gb --cpus=2 rinha-image
```

# Links

[Twitter](https://twitter.com/lucaspsilveiras)  
[LinkedIn](https://www.linkedin.com/in/lucas-pachecos/)  
[Blog](https://lucaspacheco.dev/)  