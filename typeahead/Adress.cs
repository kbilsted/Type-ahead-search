namespace typeahead
{
    class Adress
    {
        public string adresseringsvejnavn;
        public string husnr;
        public string etage;
        public string dør;
        public string supplerendebynavn;
        public string postnr;
        public string postnrnavn;

        public override string ToString() => $"::{adresseringsvejnavn} {husnr} [{etage}{dør}] {postnr} {postnrnavn} ({supplerendebynavn})::";
    }
}
