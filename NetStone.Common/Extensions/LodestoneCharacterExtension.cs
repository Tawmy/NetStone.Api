using NetStone.Model.Parseables.Character;
using NetStone.StaticData;

namespace NetStone.Common.Extensions;

public static class LodestoneCharacterExtension
{
    /// <summary>
    ///     Convert a job icon URL to its respective ClassJob
    /// </summary>
    /// <param name="character">Character to get the active ClassJob of</param>
    /// <returns>The user's active <see cref="ArgumentOutOfRangeException" /></returns>
    /// <exception cref="ClassJob">Thrown if the URL does not belong to any job</exception>
    public static ClassJob GetActiveClassJob(this LodestoneCharacter character)
    {
        if (string.IsNullOrWhiteSpace(character.ActiveClassJobIcon)) return ClassJob.None;

        return character.ActiveClassJobIcon switch
        {
            "https://lds-img.finalfantasyxiv.com/h/U/F5JzG9RPIKFSogtaKNBk455aYA.png" => ClassJob.Gladiator,
            "https://lds-img.finalfantasyxiv.com/h/V/iW7IBKQ7oglB9jmbn6LwdZXkWw.png" => ClassJob.Pugilist,
            "https://lds-img.finalfantasyxiv.com/h/N/St9rjDJB3xNKGYg-vwooZ4j6CM.png" => ClassJob.Marauder,
            "https://lds-img.finalfantasyxiv.com/h/k/tYTpoSwFLuGYGDJMff8GEFuDQs.png" => ClassJob.Lancer,
            "https://lds-img.finalfantasyxiv.com/h/Q/ZpqEJWYHj9SvHGuV9cIyRNnIkk.png" => ClassJob.Archer,
            "https://lds-img.finalfantasyxiv.com/h/s/gl62VOTBJrm7D_BmAZITngUEM8.png" => ClassJob.Conjurer,
            "https://lds-img.finalfantasyxiv.com/h/4/IM3PoP6p06GqEyReygdhZNh7fU.png" => ClassJob.Thaumaturge,
            "https://lds-img.finalfantasyxiv.com/h/v/YCN6F-xiXf03Ts3pXoBihh2OBk.png" => ClassJob.Carpenter,
            "https://lds-img.finalfantasyxiv.com/h/5/EEHVV5cIPkOZ6v5ALaoN5XSVRU.png" => ClassJob.Blacksmith,
            "https://lds-img.finalfantasyxiv.com/h/G/Rq5wcK3IPEaAB8N-T9l6tBPxCY.png" => ClassJob.Armorer,
            "https://lds-img.finalfantasyxiv.com/h/L/LbEjgw0cwO_2gQSmhta9z03pjM.png" => ClassJob.Goldsmith,
            "https://lds-img.finalfantasyxiv.com/h/b/ACAcQe3hWFxbWRVPqxKj_MzDiY.png" => ClassJob.Leatherworker,
            "https://lds-img.finalfantasyxiv.com/h/X/E69jrsOMGFvFpCX87F5wqgT_Vo.png" => ClassJob.Weaver,
            "https://lds-img.finalfantasyxiv.com/h/C/bBVQ9IFeXqjEdpuIxmKvSkqalE.png" => ClassJob.Alchemist,
            "https://lds-img.finalfantasyxiv.com/h/m/1kMI2v_KEVgo30RFvdFCyySkFo.png" => ClassJob.Culinarian,
            "https://lds-img.finalfantasyxiv.com/h/A/aM2Dd6Vo4HW_UGasK7tLuZ6fu4.png" => ClassJob.Miner,
            "https://lds-img.finalfantasyxiv.com/h/I/jGRnjIlwWridqM-mIPNew6bhHM.png" => ClassJob.Botanist,
            "https://lds-img.finalfantasyxiv.com/h/x/B4Azydbn7Prubxt7OL9p1LZXZ0.png" => ClassJob.Fisher,
            "https://lds-img.finalfantasyxiv.com/h/E/d0Tx-vhnsMYfYpGe9MvslemEfg.png" => ClassJob.Paladin,
            "https://lds-img.finalfantasyxiv.com/h/K/HW6tKOg4SOJbL8Z20GnsAWNjjM.png" => ClassJob.Monk,
            "https://lds-img.finalfantasyxiv.com/h/y/A3UhbjZvDeN3tf_6nJ85VP0RY0.png" => ClassJob.Warrior,
            "https://lds-img.finalfantasyxiv.com/h/m/gX4OgBIHw68UcMU79P7LYCpldA.png" => ClassJob.Dragoon,
            "https://lds-img.finalfantasyxiv.com/h/F/KWI-9P3RX_Ojjn_mwCS2N0-3TI.png" => ClassJob.Bard,
            "https://lds-img.finalfantasyxiv.com/h/7/i20QvSPcSQTybykLZDbQCgPwMw.png" => ClassJob.WhiteMage,
            "https://lds-img.finalfantasyxiv.com/h/P/V01m8YRBYcIs5vgbRtpDiqltSE.png" => ClassJob.BlackMage,
            "https://lds-img.finalfantasyxiv.com/h/e/VYP1LKTDpt8uJVvUT7OKrXNL9E.png" => ClassJob.Arcanist,
            "https://lds-img.finalfantasyxiv.com/h/h/4ghjpyyuNelzw1Bl0sM_PBA_FE.png" => ClassJob.Summoner,
            "https://lds-img.finalfantasyxiv.com/h/7/WdFey0jyHn9Nnt1Qnm-J3yTg5s.png" => ClassJob.Scholar,
            "https://lds-img.finalfantasyxiv.com/h/y/wdwVVcptybfgSruoh8R344y_GA.png" => ClassJob.Rogue,
            "https://lds-img.finalfantasyxiv.com/h/0/Fso5hanZVEEAaZ7OGWJsXpf3jw.png" => ClassJob.Ninja,
            "https://lds-img.finalfantasyxiv.com/h/E/vmtbIlf6Uv8rVp2YFCWA25X0dc.png" => ClassJob.Machinist,
            "https://lds-img.finalfantasyxiv.com/h/l/5CZEvDOMYMyVn2td9LZigsgw9s.png" => ClassJob.DarkKnight,
            "https://lds-img.finalfantasyxiv.com/h/1/erCgjnMSiab4LiHpWxVc-tXAqk.png" => ClassJob.Astrologian,
            "https://lds-img.finalfantasyxiv.com/h/m/KndG72XtCFwaq1I1iqwcmO_0zc.png" => ClassJob.Samurai,
            "https://lds-img.finalfantasyxiv.com/h/q/s3MlLUKmRAHy0pH57PnFStHmIw.png" => ClassJob.RedMage,
            "https://lds-img.finalfantasyxiv.com/h/p/jdV3RRKtWzgo226CC09vjen5sk.png" => ClassJob.BlueMage,
            "https://lds-img.finalfantasyxiv.com/h/8/hg8ofSSOKzqng290No55trV4mI.png" => ClassJob.Gunbreaker,
            "https://lds-img.finalfantasyxiv.com/h/t/HK0jQ1y7YV9qm30cxGOVev6Cck.png" => ClassJob.Dancer,
            "https://lds-img.finalfantasyxiv.com/h/7/cLlXUaeMPJDM2nBhIeM-uDmPzM.png" => ClassJob.Reaper,
            "https://lds-img.finalfantasyxiv.com/h/g/_oYApASVVReLLmsokuCJGkEpk0.png" => ClassJob.Sage,
            _ => throw new ArgumentOutOfRangeException(nameof(character), character.ActiveClassJobIcon,
                "the URL does not belong to any job")
        };
    }
}