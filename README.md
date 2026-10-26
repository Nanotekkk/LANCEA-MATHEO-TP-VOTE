# Vote préférentiel

Ce projet :
- il télécharge un fichier JSON de votes
- il lit les bulletins
- il élimine les candidats les moins bien classés
- il affiche le gagnant

## Ce qu'il faut savoir

Il y a 3 dossiers :
- `VotePreferentiel/` : logique du vote
- `VotePreferentiel.Worker/` : télécharge le fichier et lance le calcul
- `VotePreferentiel.Tests/` : tests pour vérifier que le code fonctionne

## Source des votes
Le JSON utilisé est ici :
https://raw.githubusercontent.com/JLou/dotnet-course/refs/heads/main/eval-1/votes.json  

## Comment lancer le projet
Ouvre un terminal dans le dossier du projet et exécute :

### 1) Tester le code
```bash
dotnet run --project "VotePreferentiel.Tests\VotePreferentiel.Tests.csproj"
```

### 2) Lancer le worker
```bash
dotnet run --project "VotePreferentiel.Worker\VotePreferentiel.Worker.csproj"
```

### 3) Compiler tout le projet
```bash
dotnet build "VotePreferentiel.slnx"
```
