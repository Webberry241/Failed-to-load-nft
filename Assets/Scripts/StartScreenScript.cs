using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using System.Threading.Tasks;

public class StartScreenScript : MonoBehaviour
{
    private ThirdwebSDK sdk;
    public GameObject HasNFT;
    public GameObject NoNFT;
    public Prefab_NFT nftPrefab;

    // Start is called before the first frame update
    void Start()
    {
        sdk = new ThirdwebSDK("polygon");

        HasNFT.SetActive(false);
        NoNFT.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void toggleStartScreen(GameObject ConnectedState, GameObject DisconnectedState, string address)
    {
        ConnectedState.SetActive(true);
        DisconnectedState.SetActive(false);

        Contract contract = sdk.GetContract("0xc08F4E1aE9B1F2c32a4548ac2B846e82C377FD01");

        string stringBalance = await checkBalance(address, contract);
        float floatBalance = float.Parse(stringBalance);

        if(floatBalance > 0)
        {
            HasNFT.SetActive(true);
        }
        else
        {
            NoNFT.SetActive(true);
            getNFTMedia(contract);
        }
    }

    public async Task<string> checkBalance(string address, Contract contract)
    {
        string balance = await contract.Read<string>("balanceOf", address, 0);
        print(balance);
        return balance;
    }

    public async void getNFTMedia(Contract contract)
    {
        NFT nft = await contract.ERC1155.Get("0");
        Prefab_NFT nftPrefabScript = nftPrefab.GetComponent<Prefab_NFT>();
        nftPrefabScript.LoadNFT(nft);
    }
}