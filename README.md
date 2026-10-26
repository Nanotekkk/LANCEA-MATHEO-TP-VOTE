# TP .NET - Vote préférentiel

Ce dépôt contient une solution .NET qui télécharge le fichier JSON des votes depuis GitHub, le désérialise et détermine le candidat gagnant selon une méthode de calcul par points (scrutin préférentiel).

## Structure
- `Vote preferentiel.slnx` : solution .NET
- `VotePreferentiel/` : bibliothèque de classes contenant les modèles et la logique de calcul
- `VotePreferentiel.Worker/` : service Worker qui télécharge le JSON et affiche le gagnant

## Données
- `votes.json` : fichier source de votes
- `votes.example.json` : exemple minimal

## Exécution
```bash
dotnet build "Vote preferentiel.slnx"
dotnet run --project "VotePreferentiel.Worker\VotePreferentiel.Worker.csproj"
```
