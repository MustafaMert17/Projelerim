#include <stdio.h>
#include <stdlib.h>
#include <time.h>

void soru1() {
    int sayilar[500];
    int sayilar2[500];
    int gecici, sayi;
    srand(time(NULL));

    for (int i = 0; i < 500; i++) {
        sayi = rand() % 1000 + 1;
        sayilar[i] = sayi;
        sayilar2[i] = sayi;
    }

    for (int i = 0; i < 500; i++) {
        printf("%d ", sayilar[i]);
    }

    printf("\n------------------------------------------------------------\n"); // Selection Sort

    clock_t baslangic, bitis;
    double gecen_sure;

    baslangic = clock();
    for (int i = 0; i < 499; i++) {
        for (int j = i + 1; j < 500; j++) {
            if (sayilar[j] < sayilar[i]) {
                gecici = sayilar[i];
                sayilar[i] = sayilar[j];
                sayilar[j] = gecici;
            }
        }
    }

    for (int i = 0; i < 500; i++) {
        printf("Selection Sort ile siralanmis dizi elemanlari = %d\n", sayilar[i]);
    }
    bitis = clock();
    gecen_sure = ((double)(bitis - baslangic)) / CLOCKS_PER_SEC;
    printf("Selection sort islemi %.2f saniye surdu.\n", gecen_sure);

    printf("------------------------------------------------------------\n"); // Insertion Sort

    baslangic = clock();
    for (int i = 1; i < 500; i++) {
        gecici = sayilar2[i];
        int j = i - 1;
        while (j >= 0 && sayilar2[j] > gecici) {
            sayilar2[j + 1] = sayilar2[j];
            j--;
        }
        sayilar2[j + 1] = gecici;
    }

    for (int i = 0; i < 500; i++) {
        printf("Insertion Sort ile siralanmis dizi elemanlari = %d\n", sayilar2[i]);
    }
    bitis = clock();
    gecen_sure = ((double)(bitis - baslangic)) / CLOCKS_PER_SEC;
    printf("Insertion sort islemi %.2f saniye surdu.\n", gecen_sure);


}


void soru2() {
    int gecici, sayac = 0;
    int sayilar[10] = { 13, 214, 56, 627, 123, 10, 190, 20, 32, 14 };
    int bk[10];

    // Insertion Sort
    for (int i = 0; i < 10; i++) {
        gecici = sayilar[i];
        int j = i - 1;
        while (j > -1 && sayilar[j] > gecici) {
            sayilar[j + 1] = sayilar[j];
            j--;
        }
        sayilar[j + 1] = gecici;
    }

    // Diziyi tersine çevir ve bk dizisine ata
    for (int t = 9; t >= 0; t--) {
        bk[sayac] = sayilar[t];
        sayac++;
    }

    // Ýlk beþ elemaný yazdýr
    for (int i = 0; i < 5; i++) {
        printf("%d ", bk[i]);
        printf("%d ", sayilar[i]);
    }


}
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>

#define ALPHABET_SIZE 26

// Trie düðüm yapýsý
typedef struct TrieNode {
    struct TrieNode* children[ALPHABET_SIZE];
    bool isEndOfWord; // Bu düðüm bir kelimenin sonunu gösterir
} TrieNode;

// Yeni bir Trie düðümü oluþtur
TrieNode* createNode() {
    TrieNode* node = (TrieNode*)malloc(sizeof(TrieNode));
    node->isEndOfWord = false;
    for (int i = 0; i < ALPHABET_SIZE; i++) {
        node->children[i] = NULL;
    }
    return node;
}

// Ekleme Ýþlemi
void insert(TrieNode* root, const char* key) {
    TrieNode* crawler = root;
    while (*key) {
        int index = *key - 'a';
        if (crawler->children[index] == NULL) {
            crawler->children[index] = createNode();
        }
        crawler = crawler->children[index];
        key++;
    }
    crawler->isEndOfWord = true;
}

// Arama Ýþlemi
bool search(TrieNode* root, const char* key) {
    TrieNode* crawler = root;
    while (*key) {
        int index = *key - 'a';
        if (crawler->children[index] == NULL) {
            return false;
        }
        crawler = crawler->children[index];
        key++;
    }
    return crawler != NULL && crawler->isEndOfWord;
}

// Boþ olup olmadýðýný kontrol et
bool isEmpty(TrieNode* root) {
    for (int i = 0; i < ALPHABET_SIZE; i++) {
        if (root->children[i] != NULL) {
            return false;
        }
    }
    return true;
}

// Silme Ýþlemi (Rekürsif)
TrieNode* Delete(TrieNode* root, const char* key, int depth) {
    if (!root) {
        return NULL;
    }

    if (depth == strlen(key)) {
        if (root->isEndOfWord) {
            root->isEndOfWord = false;
        }
        if (isEmpty(root)) {
            free(root);
            root = NULL;
        }
        return root;
    }

    int index = key[depth] - 'a';
    root->children[index] = Delete(root->children[index], key, depth + 1);

    if (isEmpty(root) && !root->isEndOfWord) {
        free(root);
        root = NULL;
    }

    return root;
}

// Kullaným Örneði
void soru3() {
    char keys[][8] = { "the", "a", "there", "answer", "any", "by", "bye", "their" };
    int n = sizeof(keys) / sizeof(keys[0]);

    TrieNode* root = createNode();

    // Kelimeleri ekle
    for (int i = 0; i < n; i++) {
        insert(root, keys[i]);
    }

    // Kelimeleri ara
    search(root, "the") ? printf("the -- found\n") : printf("the -- not found\n");
    search(root, "these") ? printf("these -- found\n") : printf("these -- not found\n");

    // Kelime sil
    root = Delete(root, "the", 0);

    // Silindikten sonra tekrar arama
    search(root, "the") ? printf("the -- found\n") : printf("the -- not found\n");


}
int main(void) {
    printf(" \n");
    soru2();
    system("pause");

}